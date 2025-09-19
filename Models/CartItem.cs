using System.ComponentModel.DataAnnotations;

namespace GocPho.Models;

public class CartItem
{
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    
    [Required(ErrorMessage = "Số lượng là bắt buộc")]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int Quantity { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Calculated property
    public decimal TotalPrice => Product?.Price * Quantity ?? 0;
}
