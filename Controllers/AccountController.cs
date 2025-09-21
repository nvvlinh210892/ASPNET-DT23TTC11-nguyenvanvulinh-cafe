using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GocPho.Models;

namespace GocPho.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    // GET: Account/Register
    public IActionResult Register()
    {
        return View();
    }

    // POST: Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Thêm role Customer cho user mới
                await _userManager.AddToRoleAsync(user, "Customer");

                _logger.LogInformation("User created a new account with password.");

                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["Success"] = "Đăng ký thành công! Chào mừng bạn đến với Góc phố Coffee.";
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    // GET: Account/Login
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // POST: Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                ModelState.AddModelError(string.Empty, "Tài khoản đã bị khóa.");
                return View(model);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng.");
                return View(model);
            }
        }

        return View(model);
    }

    // POST: Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        TempData["Success"] = "Đã đăng xuất thành công.";
        return RedirectToAction("Index", "Home");
    }

    // GET: Account/Profile
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var model = new RegisterViewModel
        {
            Email = user.Email ?? "",
            FullName = user.UserName ?? "",
            PhoneNumber = user.PhoneNumber ?? ""
        };

        return View(model);
    }

    // POST: Account/Profile
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(RegisterViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            user.UserName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Cập nhật thông tin thành công!";
                return RedirectToAction(nameof(Profile));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    // GET: Account/EditProfile
    [Authorize]
    public async Task<IActionResult> EditProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var model = new RegisterViewModel
        {
            Email = user.Email ?? "",
            FullName = user.UserName ?? "",
            PhoneNumber = user.PhoneNumber ?? ""
        };

        return View(model);
    }

    // POST: Account/EditProfile
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(RegisterViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        // Remove password validation for profile update
        ModelState.Remove("Password");
        ModelState.Remove("ConfirmPassword");

        if (ModelState.IsValid)
        {
            user.UserName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Cập nhật thông tin thành công!";
                return RedirectToAction(nameof(Profile));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
