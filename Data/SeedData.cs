using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GocPho.Models;

namespace GocPho.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, 
        UserManager<IdentityUser> userManager, 
        RoleManager<IdentityRole> roleManager)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();
        
        // Create roles
        await CreateRoles(roleManager);

        // Create admin user
        await CreateAdminUser(userManager);

        // Seed demo data
        // await SeedDemoData(context, userManager);
    }
    
    private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "Admin", "Customer" };
        
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
    
    private static async Task CreateAdminUser(UserManager<IdentityUser> userManager)
    {
        // THÔNG TIN TÀI KHOẢN QUẢN TRỊ VIÊN WEBSITE BÁN CAFE GÓC PHỞ
        // ========================================================
        // Email: admin@gocpho.com
        // Username: admin
        // Password: GocPho@2025! (Mật khẩu mạnh bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt)
        // Role: Admin (Có quyền quản lý toàn bộ hệ thống)
        // ========================================================
        
        var adminEmail = "admin@gocpho.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        
        if (adminUser == null)
        {
            var newAdminUser = new IdentityUser
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true
            };
            
            // Sử dụng mật khẩu mạnh: GocPho@2025!
            var result = await userManager.CreateAsync(newAdminUser, "GocPho@2025!");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdminUser, "Admin");
            }
        }
    }

    private static async Task SeedDemoData(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        // Seed demo customers
        await SeedDemoCustomers(context, userManager);

        // Seed demo orders
        await SeedDemoOrders(context);
    }

    private static async Task SeedDemoCustomers(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        var demoCustomers = new[]
        {
            new { Email = "customer1@example.com", Name = "Nguyễn Văn A", Phone = "0901234567" },
            new { Email = "customer2@example.com", Name = "Trần Thị B", Phone = "0902345678" },
            new { Email = "customer3@example.com", Name = "Lê Văn C", Phone = "0903456789" },
            new { Email = "customer4@example.com", Name = "Phạm Thị D", Phone = "0904567890" },
            new { Email = "customer5@example.com", Name = "Hoàng Văn E", Phone = "0905678901" }
        };

        foreach (var customer in demoCustomers)
        {
            var existingUser = await userManager.FindByEmailAsync(customer.Email);
            if (existingUser == null)
            {
                var newUser = new IdentityUser
                {
                    UserName = customer.Name,
                    Email = customer.Email,
                    EmailConfirmed = true,
                    PhoneNumber = customer.Phone
                };

                var result = await userManager.CreateAsync(newUser, "Customer123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "Customer");
                }
            }
        }
    }

    private static async Task SeedDemoOrders(ApplicationDbContext context)
    {
        // Only seed if no orders exist
        if (await context.Orders.AnyAsync())
            return;

        var users = await context.Users.ToListAsync();
        var products = await context.Products.ToListAsync();

        if (!users.Any() || !products.Any())
            return;

        var random = new Random();
        var orderStatuses = Enum.GetValues<OrderStatus>();

        // Create 15 demo orders
        for (int i = 0; i < 15; i++)
        {
            var user = users[random.Next(users.Count)];
            var orderDate = DateTime.Now.AddDays(-random.Next(30)); // Orders from last 30 days

            var order = new Order
            {
                UserId = user.Id,
                CustomerName = user.UserName ?? "Khách hàng",
                PhoneNumber = user.PhoneNumber ?? "0900000000",
                DeliveryAddress = $"Địa chỉ {i + 1}, Quận {random.Next(1, 13)}, TP.HCM",
                OrderDate = orderDate,
                Status = orderStatuses[random.Next(orderStatuses.Length)],
                Notes = i % 3 == 0 ? "Giao hàng nhanh" : null
            };

            // Add random products to order
            var numItems = random.Next(1, 4); // 1-3 items per order
            var selectedProducts = products.OrderBy(x => random.Next()).Take(numItems);

            decimal totalAmount = 0;
            foreach (var product in selectedProducts)
            {
                var quantity = random.Next(1, 4);
                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    UnitPrice = product.Price
                };

                order.OrderItems.Add(orderItem);
                totalAmount += quantity * product.Price;
            }

            order.TotalAmount = totalAmount;

            // Set delivery date for delivered orders
            if (order.Status == OrderStatus.Delivered)
            {
                order.DeliveryDate = orderDate.AddDays(random.Next(1, 3));
            }

            context.Orders.Add(order);
        }

        await context.SaveChangesAsync();
    }
}
