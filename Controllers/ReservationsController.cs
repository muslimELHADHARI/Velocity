using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Velocity.Models;
using Velocity.Services;
using Velocity.ViewModels;

namespace Velocity.Controllers;

[Authorize]
public class ReservationsController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReservationsController(IReservationService reservationService, UserManager<ApplicationUser> userManager)
    {
        _reservationService = reservationService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> My()
    {
        var userId = _userManager.GetUserId(User)!;
        var reservations = await _reservationService.GetForUserAsync(userId);
        return View(reservations);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int vehicleId)
    {
        var model = await _reservationService.BuildCreateModelAsync(vehicleId);
        if (model is null) return NotFound();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReservationCreateViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var userId = _userManager.GetUserId(User)!;
        var (success, error, reservation) = await _reservationService.CreateAsync(userId, model);
        if (!success || reservation is null)
        {
            ModelState.AddModelError(string.Empty, error);
            return View(model);
        }

        return RedirectToAction("Summary", "Checkout", new { id = reservation.Id });
    }
}

