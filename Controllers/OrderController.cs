using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GocPho.Data;
using GocPho.Models;
using GocPho.Services;

namespace GocPho.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly CartService _cartService;
    private readonly UserManager<IdentityUser> _userManager;

    public OrderController(ApplicationDbContext context, CartService cartService, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _cartService = cartService;
        _userManager = userManager;
    }

    // GET: Order/Checkout
    public async Task<IActionResult> Checkout()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var cartItems = await _cartService.GetCartItemsAsync(userId);
        if (!cartItems.Any())
        {
            TempData["Error"] = "Giỏ hàng của bạn đang trống.";
            return RedirectToAction("Index", "Cart");
        }

        var user = await _userManager.GetUserAsync(User);
        var viewModel = new CheckoutViewModel
        {
            CartItems = cartItems,
            CustomerName = user?.UserName ?? "",
            PhoneNumber = user?.PhoneNumber ?? ""
        };

        return View(viewModel);
    }

    // POST: Order/Checkout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutViewModel model)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var cartItems = await _cartService.GetCartItemsAsync(userId);
        model.CartItems = cartItems;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!cartItems.Any())
        {
            TempData["Error"] = "Giỏ hàng của bạn đang trống.";
            return RedirectToAction("Index", "Cart");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Tạo đơn hàng
            var order = new Order
            {
                UserId = userId,
                CustomerName = model.CustomerName,
                PhoneNumber = model.PhoneNumber,
                DeliveryAddress = model.DeliveryAddress,
                Notes = model.Notes ?? "",
                TotalAmount = cartItems.Sum(item => item.TotalPrice),
                Status = OrderStatus.Pending,
                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Tạo chi tiết đơn hàng
            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Product?.Price ?? 0
                };
                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();

            // Xóa giỏ hàng
            await _cartService.ClearCartAsync(userId);

            await transaction.CommitAsync();

            TempData["Success"] = "Đặt hàng thành công! Chúng tôi sẽ liên hệ với bạn sớm nhất.";
            return RedirectToAction("OrderSuccess", new { id = order.Id });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            TempData["Error"] = "Có lỗi xảy ra khi đặt hàng. Vui lòng thử lại.";
            return View(model);
        }
    }

    // GET: Order/OrderSuccess/5
    public async Task<IActionResult> OrderSuccess(int id)
    {
        var userId = _userManager.GetUserId(User);
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // GET: Order/MyOrders
    public async Task<IActionResult> MyOrders()
    {
        var userId = _userManager.GetUserId(User);
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        return View(orders);
    }

    // GET: Order/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var userId = _userManager.GetUserId(User);
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }
}
