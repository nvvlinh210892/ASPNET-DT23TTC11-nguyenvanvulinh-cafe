using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GocPho.Data;
using GocPho.Models;

namespace GocPho.Controllers;

[AllowAnonymous]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Admin
    public async Task<IActionResult> Index()
    {
        var viewModel = new AdminDashboardViewModel
        {
            TotalProducts = await _context.Products.CountAsync(),
            TotalOrders = await _context.Orders.CountAsync(),
            PendingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending),
            TotalRevenue = await _context.Orders
                .Where(o => o.Status == OrderStatus.Delivered)
                .SumAsync(o => o.TotalAmount),
            RecentOrders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .Take(10)
                .ToListAsync()
        };

        return View(viewModel);
    }

    // GET: Admin/Products
    public async Task<IActionResult> Products()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .ToListAsync();
        return View(products);
    }

    // GET: Admin/CreateProduct
    public async Task<IActionResult> CreateProduct()
    {
        ViewBag.Categories = await _context.Categories.ToListAsync();
        return View();
    }

    // POST: Admin/CreateProduct
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            product.CreatedAt = DateTime.Now;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Sản phẩm đã được tạo thành công.";
            return RedirectToAction(nameof(Products));
        }

        ViewBag.Categories = await _context.Categories.ToListAsync();
        return View(product);
    }

    // GET: Admin/EditProduct/5
    public async Task<IActionResult> EditProduct(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        ViewBag.Categories = await _context.Categories.ToListAsync();
        return View(product);
    }

    // POST: Admin/EditProduct/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Sản phẩm đã được cập nhật thành công.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Products));
        }

        ViewBag.Categories = await _context.Categories.ToListAsync();
        return View(product);
    }

    // POST: Admin/DeleteProduct/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Sản phẩm đã được xóa thành công.";
        }

        return RedirectToAction(nameof(Products));
    }

    // GET: Admin/Orders
    public async Task<IActionResult> Orders(OrderStatus? status)
    {
        var orders = _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).AsQueryable();

        if (status.HasValue)
        {
            orders = orders.Where(o => o.Status == status.Value);
        }

        ViewBag.CurrentStatus = status;
        return View(await orders.OrderByDescending(o => o.OrderDate).ToListAsync());
    }

    // GET: Admin/OrderDetails/5
    public async Task<IActionResult> OrderDetails(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // POST: Admin/UpdateOrderStatus
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus status)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order != null)
        {
            order.Status = status;
            if (status == OrderStatus.Delivered)
            {
                order.DeliveryDate = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            TempData["Success"] = "Trạng thái đơn hàng đã được cập nhật.";
        }

        return RedirectToAction(nameof(Orders));
    }

    // GET: Admin/Users
    public async Task<IActionResult> Users()
    {
        var users = await _userManager.Users.ToListAsync();
        var userViewModels = new List<UserViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userViewModels.Add(new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber ?? "",
                EmailConfirmed = user.EmailConfirmed,
                Roles = roles.ToList()
            });
        }

        return View(userViewModels);
    }

    // GET: Admin/Categories
    public async Task<IActionResult> Categories()
    {
        var categories = await _context.Categories.ToListAsync();

        // Get product counts for each category
        var productCounts = await _context.Products
            .GroupBy(p => p.CategoryId)
            .Select(g => new { CategoryId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.CategoryId, x => x.Count);

        ViewBag.ProductCounts = productCounts;

        return View(categories);
    }

    // GET: Admin/CreateCategory
    public IActionResult CreateCategory()
    {
        return View();
    }

    // POST: Admin/CreateCategory
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Danh mục đã được tạo thành công.";
            return RedirectToAction(nameof(Categories));
        }
        return View(category);
    }

    // GET: Admin/EditCategory/5
    public async Task<IActionResult> EditCategory(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    // POST: Admin/EditCategory/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCategory(int id, Category category)
    {
        if (id != category.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Danh mục đã được cập nhật thành công.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Categories));
        }
        return View(category);
    }

    // POST: Admin/DeleteCategory/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            // Check if category has products
            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id);
            if (hasProducts)
            {
                TempData["Error"] = "Không thể xóa danh mục này vì còn sản phẩm thuộc danh mục.";
            }
            else
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Danh mục đã được xóa thành công.";
            }
        }

        return RedirectToAction(nameof(Categories));
    }

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }

    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(e => e.Id == id);
    }

    // Action to update product images with real coffee photos
    public async Task<IActionResult> UpdateToRealImages()
    {
        try
        {
            var updates = new Dictionary<int, string>
            {
                { 1, "https://upload.wikimedia.org/wikipedia/commons/thumb/4/45/A_small_cup_of_coffee.JPG/300px-A_small_cup_of_coffee.JPG" }, // Cà phê đen đá
                { 2, "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c8/Cappuccino_at_Sightglass_Coffee.jpg/300px-Cappuccino_at_Sightglass_Coffee.jpg" }, // Cà phê sữa đá
                { 3, "https://upload.wikimedia.org/wikipedia/commons/thumb/1/16/Latte_and_dark_coffee.jpg/300px-Latte_and_dark_coffee.jpg" }, // Bạc xỉu
                { 4, "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c8/Cappuccino_at_Sightglass_Coffee.jpg/300px-Cappuccino_at_Sightglass_Coffee.jpg" }, // Cappuccino
                { 5, "https://upload.wikimedia.org/wikipedia/commons/thumb/0/06/Americano_Coffee.jpg/300px-Americano_Coffee.jpg" }, // Americano
                { 6, "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2f/Iced_Coffee.jpg/300px-Iced_Coffee.jpg" }, // Frappuccino
                { 7, "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a5/Tazzina_di_caff%C3%A8_a_Ventimiglia.jpg/300px-Tazzina_di_caff%C3%A8_a_Ventimiglia.jpg" }, // Espresso
                { 8, "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9f/Latte_at_Doppio_Ristretto_Chiang_Mai_01.jpg/300px-Latte_at_Doppio_Ristretto_Chiang_Mai_01.jpg" }, // Latte
                { 9, "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f6/Mocaccino-Coffee.jpg/300px-Mocaccino-Coffee.jpg" }  // Mocha
            };

            int updatedCount = 0;
            foreach (var update in updates)
            {
                var product = await _context.Products.FindAsync(update.Key);
                if (product != null)
                {
                    product.ImageUrl = update.Value;
                    updatedCount++;
                }
            }

            await _context.SaveChangesAsync();

            return Json(new {
                success = true,
                message = $"Đã cập nhật hình ảnh thực tế cho {updatedCount} sản phẩm!",
                updatedCount = updatedCount
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Lỗi: " + ex.Message });
        }
    }

    // Action to reset admin account
    public async Task<IActionResult> ResetAdminAccount()
    {
        try
        {
            var userManager = HttpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure Admin role exists
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Find existing admin user
            var adminEmail = "admin@gocpho.com";
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin != null)
            {
                // Delete existing admin
                await userManager.DeleteAsync(existingAdmin);
            }

            // Create new admin user
            var newAdminUser = new IdentityUser
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(newAdminUser, "admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdminUser, "Admin");
                return Json(new {
                    success = true,
                    message = "Tài khoản admin đã được reset thành công!\nEmail: admin@gocpho.com\nUsername: admin\nPassword: admin@123"
                });
            }
            else
            {
                return Json(new {
                    success = false,
                    message = "Lỗi tạo tài khoản: " + string.Join(", ", result.Errors.Select(e => e.Description))
                });
            }
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Lỗi: " + ex.Message });
        }
    }
}
