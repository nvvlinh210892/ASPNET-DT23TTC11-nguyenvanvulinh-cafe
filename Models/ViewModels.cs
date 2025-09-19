using System.ComponentModel.DataAnnotations;

namespace GocPho.Models;

// ViewModel cho đăng ký
public class RegisterViewModel
{
    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Họ tên là bắt buộc")]
    [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
    public string FullName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự và tối đa {1} ký tự.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

// ViewModel cho đăng nhập
public class LoginViewModel
{
    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    
    public bool RememberMe { get; set; }
}

// ViewModel cho giỏ hàng
public class CartViewModel
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();
    public decimal TotalAmount => Items.Sum(item => item.TotalPrice);
    public int TotalItems => Items.Sum(item => item.Quantity);
}

// ViewModel cho checkout
public class CheckoutViewModel
{
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
    public string Notes { get; set; } = string.Empty;
    
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    public decimal TotalAmount => CartItems.Sum(item => item.TotalPrice);
}

// ViewModel cho trang chủ
public class HomeViewModel
{
    public List<Product> FeaturedProducts { get; set; } = new List<Product>();
    public List<Category> Categories { get; set; } = new List<Category>();
}

// ViewModel cho admin dashboard
public class AdminDashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<Order> RecentOrders { get; set; } = new List<Order>();
}
