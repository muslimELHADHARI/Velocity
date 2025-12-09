using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Velocity.Services;

namespace Velocity.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;
    private readonly IReservationService _reservationService;

    public AdminController(IAdminService adminService, IReservationService reservationService)
    {
        _adminService = adminService;
        _reservationService = reservationService;
    }

    public async Task<IActionResult> Index()
    {
        var vm = await _adminService.GetDashboardAsync();
        return View(vm);
    }

    public async Task<IActionResult> Users()
    {
        var users = await _adminService.GetUsersAsync();
        return View(users);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateUser(string userId, string role, bool isActive)
    {
        await _adminService.UpdateUserRoleAsync(userId, role, isActive);
        TempData["Success"] = "User updated.";
        return RedirectToAction(nameof(Users));
    }

    public async Task<IActionResult> Reservations()
    {
        var reservations = await _reservationService.GetAllAsync();
        return View(reservations);
    }
}

