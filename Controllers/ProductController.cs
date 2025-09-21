using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GocPho.Data;
using GocPho.Models;

namespace GocPho.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Product
    public async Task<IActionResult> Index(int? categoryId, string searchString)
    {
        var products = _context.Products.Include(p => p.Category).Where(p => p.IsAvailable);

        if (categoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrEmpty(searchString))
        {
            products = products.Where(p => p.Name.Contains(searchString) || 
                                          p.Description.Contains(searchString));
        }

        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.CurrentCategory = categoryId;
        ViewBag.SearchString = searchString;

        return View(await products.ToListAsync());
    }

    // GET: Product/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Product/Menu
    public async Task<IActionResult> Menu(int? categoryId)
    {
        var products = _context.Products.Include(p => p.Category).Where(p => p.IsAvailable);

        if (categoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == categoryId.Value);
        }

        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.CurrentCategory = categoryId;

        return View(await products.ToListAsync());
    }

    // Debug action to check product images
    public async Task<IActionResult> Debug()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .ToListAsync();

        var debugInfo = products.Select(p => new {
            Id = p.Id,
            Name = p.Name,
            ImageUrl = p.ImageUrl,
            Category = p.Category?.Name
        }).ToList();

        return Json(debugInfo);
    }

    // Force update product images
    public async Task<IActionResult> UpdateImages()
    {
        var products = await _context.Products.ToListAsync();

        foreach (var product in products)
        {
            switch (product.Id)
            {
                case 1:
                    product.ImageUrl = "/images/ca-phe-den-da.svg";
                    break;
                case 2:
                    product.ImageUrl = "/images/ca-phe-sua-da.svg";
                    break;
                case 3:
                    product.ImageUrl = "/images/bac-xiu.svg";
                    break;
                default:
                    product.ImageUrl = "/images/default-coffee.svg";
                    break;
            }
        }

        await _context.SaveChangesAsync();

        return Json(new { message = "Images updated successfully", products = products.Select(p => new { p.Id, p.Name, p.ImageUrl }) });
    }
}
