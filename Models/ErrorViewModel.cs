using System.ComponentModel.DataAnnotations;

namespace GocPho.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

// Models cho sản phẩm cà phê
public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
    [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Giá là bắt buộc")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public bool IsAvailable { get; set; } = true;

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation properties
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

// Models cho danh mục sản phẩm
public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
    [StringLength(50, ErrorMessage = "Tên danh mục không được vượt quá 50 ký tự")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Mô tả không được vượt quá 200 ký tự")]
    public string Description { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
