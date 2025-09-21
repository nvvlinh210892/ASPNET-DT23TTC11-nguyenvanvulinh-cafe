using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GocPho.Models;

namespace GocPho.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Cấu hình cho Product
        builder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ImageUrl).HasMaxLength(200);
            
            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Cấu hình cho Category
        builder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
        });

        // Cấu hình cho CartItem
        builder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            
            entity.HasOne(e => e.Product)
                  .WithMany(p => p.CartItems)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Cấu hình cho Order
        builder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.DeliveryAddress).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
        });

        // Cấu hình cho OrderItem
        builder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
            
            entity.HasOne(e => e.Order)
                  .WithMany(o => o.OrderItems)
                  .HasForeignKey(e => e.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Product)
                  .WithMany(p => p.OrderItems)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed data cho Categories
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Cà phê đen", Description = "Các loại cà phê đen truyền thống" },
            new Category { Id = 2, Name = "Cà phê sữa", Description = "Các loại cà phê pha với sữa" },
            new Category { Id = 3, Name = "Cà phê đá xay", Description = "Các loại cà phê đá xay mát lạnh" },
            new Category { Id = 4, Name = "Đồ uống khác", Description = "Trà, nước ép và các đồ uống khác" }
        );

        // Seed data cho Products
        builder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Cà phê đen đá", Description = "Cà phê đen truyền thống với đá, thơm ngon đậm đà", Price = 25000, CategoryId = 1, ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/45/A_small_cup_of_coffee.JPG/300px-A_small_cup_of_coffee.JPG", IsAvailable = true, CreatedAt = new DateTime(2025, 8, 15) },
            new Product { Id = 2, Name = "Cà phê sữa đá", Description = "Cà phê sữa đá thơm ngon, vị ngọt dịu", Price = 30000, CategoryId = 2, ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c8/Cappuccino_at_Sightglass_Coffee.jpg/300px-Cappuccino_at_Sightglass_Coffee.jpg", IsAvailable = true, CreatedAt = new DateTime(2025, 8, 16) },
            new Product { Id = 3, Name = "Bạc xỉu", Description = "Cà phê sữa đậm đà phong cách Sài Gòn", Price = 35000, CategoryId = 2, ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/16/Latte_and_dark_coffee.jpg/300px-Latte_and_dark_coffee.jpg", IsAvailable = true, CreatedAt = new DateTime(2025, 8, 18) },
            new Product { Id = 4, Name = "Cappuccino", Description = "Cà phê Cappuccino Ý với lớp foam mịn màng", Price = 45000, CategoryId = 2, ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c8/Cappuccino_at_Sightglass_Coffee.jpg/300px-Cappuccino_at_Sightglass_Coffee.jpg", IsAvailable = true, CreatedAt = new DateTime(2025, 8, 20) },
            new Product { Id = 5, Name = "Americano", Description = "Cà phê Americano đậm chất, hương vị tinh tế", Price = 40000, CategoryId = 1, ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/06/Americano_Coffee.jpg/300px-Americano_Coffee.jpg", IsAvailable = true, CreatedAt = new DateTime(2025, 8, 22) },
            new Product { Id = 6, Name = "Frappuccino", Description = "Cà phê đá xay mát lạnh với whipped cream", Price = 50000, CategoryId = 3, ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2f/Iced_Coffee.jpg/300px-Iced_Coffee.jpg", IsAvailable = true, CreatedAt = new DateTime(2025, 8, 25) },
            new Product { Id = 7, Name = "Espresso", Description = "Cà phê Espresso đậm đà nguyên chất từ Ý", Price = 35000, CategoryId = 1, ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a5/Tazzina_di_caff%C3%A8_a_Ventimiglia.jpg/300px-Tazzina_di_caff%C3%A8_a_Ventimiglia.jpg", IsAvailable = true, CreatedAt = new DateTime(2025, 8, 27) },
            new Product { Id = 8, Name = "Latte", Description = "Cà phê Latte với lớp sữa mịn màng và latte art", Price = 42000, CategoryId = 2, ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9f/Latte_at_Doppio_Ristretto_Chiang_Mai_01.jpg/300px-Latte_at_Doppio_Ristretto_Chiang_Mai_01.jpg", IsAvailable = true, CreatedAt = new DateTime(2025, 8, 28) },
            new Product { Id = 9, Name = "Mocha", Description = "Cà phê Mocha với chocolate thơm ngon và whipped cream", Price = 48000, CategoryId = 2, ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f6/Mocaccino-Coffee.jpg/300px-Mocaccino-Coffee.jpg", IsAvailable = true, CreatedAt = new DateTime(2025, 8, 30) }
        );
    }
}
