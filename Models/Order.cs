using System.ComponentModel.DataAnnotations;

namespace GocPho.Models;

public class Order
{
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Họ tên là bắt buộc")]
    [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
    public string CustomerName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Địa chỉ giao hàng là bắt buộc")]
    [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
    public string DeliveryAddress { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
    public string? Notes { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    
    public DateTime OrderDate { get; set; } = DateTime.Now;
    
    public DateTime? DeliveryDate { get; set; }
    
    // Navigation properties
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

public enum OrderStatus
{
    Pending = 0,        // Chờ xác nhận
    Confirmed = 1,      // Đã xác nhận
    Preparing = 2,      // Đang chuẩn bị
    Delivering = 3,     // Đang giao hàng
    Delivered = 4,      // Đã giao hàng
    Cancelled = 5       // Đã hủy
}

public class OrderItem
{
    public int Id { get; set; }
    
    public int OrderId { get; set; }
    public Order? Order { get; set; }
    
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    
    [Required(ErrorMessage = "Số lượng là bắt buộc")]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    // Calculated property
    public decimal TotalPrice => UnitPrice * Quantity;
}
