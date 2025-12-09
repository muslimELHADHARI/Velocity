using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Velocity.Models;
using Velocity.ViewModels;

namespace Velocity.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        var model = new ProfileViewModel
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber,
            UserName = user.UserName ?? string.Empty,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive
        };

        ViewData["UserRole"] = roles.FirstOrDefault() ?? "Customer";
        return View(model);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                ViewData["UserRole"] = roles.FirstOrDefault() ?? "Customer";
            }
            return View("Index", model);
        }

        var userToUpdate = await _userManager.FindByIdAsync(model.Id);
        if (userToUpdate == null || userToUpdate.Id != _userManager.GetUserId(User))
        {
            return Forbid();
        }

        userToUpdate.Email = model.Email;
        userToUpdate.UserName = model.Email;
        userToUpdate.PhoneNumber = model.PhoneNumber;

        var result = await _userManager.UpdateAsync(userToUpdate);
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        var roles2 = await _userManager.GetRolesAsync(userToUpdate);
        ViewData["UserRole"] = roles2.FirstOrDefault() ?? "Customer";
        return View("Index", model);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var profileModel = new ProfileViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName ?? string.Empty,
                    CreatedAt = user.CreatedAt,
                    IsActive = user.IsActive
                };
                var roles = await _userManager.GetRolesAsync(user);
                ViewData["UserRole"] = roles.FirstOrDefault() ?? "Customer";
                ViewData["ShowPasswordForm"] = true;
                return View("Index", profileModel);
            }
            return RedirectToAction("Index");
        }

        var userToUpdate = await _userManager.GetUserAsync(User);
        if (userToUpdate == null) return NotFound();

        var result = await _userManager.ChangePasswordAsync(userToUpdate, model.CurrentPassword, model.NewPassword);
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Password changed successfully!";
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        var profileModel2 = new ProfileViewModel
        {
            Id = userToUpdate.Id,
            Email = userToUpdate.Email ?? string.Empty,
            PhoneNumber = userToUpdate.PhoneNumber,
            UserName = userToUpdate.UserName ?? string.Empty,
            CreatedAt = userToUpdate.CreatedAt,
            IsActive = userToUpdate.IsActive
        };
        var roles2 = await _userManager.GetRolesAsync(userToUpdate);
        ViewData["UserRole"] = roles2.FirstOrDefault() ?? "Customer";
        ViewData["ShowPasswordForm"] = true;
        return View("Index", profileModel2);
    }

    [HttpGet]
    public IActionResult Register(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("User created a new account with password.");
                return RedirectToLocal(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
}

