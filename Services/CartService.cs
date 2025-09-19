using GocPho.Data;
using GocPho.Models;
using Microsoft.EntityFrameworkCore;

namespace GocPho.Services;

public class CartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CartItem>> GetCartItemsAsync(string userId)
    {
        return await _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task AddToCartAsync(string userId, int productId, int quantity = 1)
    {
        var existingItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            _context.CartItems.Update(existingItem);
        }
        else
        {
            var cartItem = new CartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity,
                CreatedAt = DateTime.Now
            };
            _context.CartItems.Add(cartItem);
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdateCartItemAsync(string userId, int productId, int quantity)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

        if (cartItem != null)
        {
            if (quantity <= 0)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
                _context.CartItems.Update(cartItem);
            }

            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveFromCartAsync(string userId, int productId)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(string userId)
    {
        var cartItems = await _context.CartItems
            .Where(c => c.UserId == userId)
            .ToListAsync();

        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetCartItemCountAsync(string userId)
    {
        return await _context.CartItems
            .Where(c => c.UserId == userId)
            .SumAsync(c => c.Quantity);
    }

    public async Task<decimal> GetCartTotalAsync(string userId)
    {
        var cartItems = await GetCartItemsAsync(userId);
        return cartItems.Sum(item => item.TotalPrice);
    }
}
