using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GocPho.Models;
using GocPho.Services;

namespace GocPho.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly CartService _cartService;
    private readonly UserManager<IdentityUser> _userManager;

    public CartController(CartService cartService, UserManager<IdentityUser> userManager)
    {
        _cartService = cartService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var cartItems = await _cartService.GetCartItemsAsync(userId);
        var viewModel = new CartViewModel
        {
            Items = cartItems
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Json(new { success = false, message = "Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng." });
        }

        try
        {
            await _cartService.AddToCartAsync(userId, productId, quantity);
            var cartCount = await _cartService.GetCartItemCountAsync(userId);
            
            return Json(new { success = true, message = "Đã thêm sản phẩm vào giỏ hàng!", cartCount = cartCount });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Json(new { success = false, message = "Vui lòng đăng nhập." });
        }

        try
        {
            await _cartService.UpdateCartItemAsync(userId, productId, quantity);
            var cartTotal = await _cartService.GetCartTotalAsync(userId);
            var cartCount = await _cartService.GetCartItemCountAsync(userId);
            
            return Json(new { success = true, cartTotal = cartTotal, cartCount = cartCount });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật giỏ hàng." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Json(new { success = false, message = "Vui lòng đăng nhập." });
        }

        try
        {
            await _cartService.RemoveFromCartAsync(userId, productId);
            var cartTotal = await _cartService.GetCartTotalAsync(userId);
            var cartCount = await _cartService.GetCartItemCountAsync(userId);
            
            return Json(new { success = true, message = "Đã xóa sản phẩm khỏi giỏ hàng!", cartTotal = cartTotal, cartCount = cartCount });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Có lỗi xảy ra khi xóa sản phẩm." });
        }
    }

    public async Task<IActionResult> GetCartCount()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Json(new { count = 0 });
        }

        var count = await _cartService.GetCartItemCountAsync(userId);
        return Json(new { count = count });
    }
}
