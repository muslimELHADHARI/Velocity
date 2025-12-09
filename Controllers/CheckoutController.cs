using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Velocity.Models.Enums;
using Velocity.Services;

namespace Velocity.Controllers;

[Authorize]
public class CheckoutController : Controller
{
    private readonly ICheckoutService _checkoutService;
    private readonly IReservationService _reservationService;

    public CheckoutController(ICheckoutService checkoutService, IReservationService reservationService)
    {
        _checkoutService = checkoutService;
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<IActionResult> Summary(int id)
    {
        var vm = await _checkoutService.BuildSummaryAsync(id);
        if (vm is null) return NotFound();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PayOnline(int id, string cardHolder, string cardNumber)
    {
        var success = await _checkoutService.ProcessPaymentAsync(id, cardHolder, cardNumber);
        TempData["Success"] = success ? "Payment successful" : "Payment failed, please try again.";
        if (success)
        {
            await _reservationService.UpdateStatusAsync(id, true);
        }

        return RedirectToAction(nameof(Summary), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PayOnSite(int id)
    {
        await _reservationService.UpdateStatusAsync(id, false);
        TempData["Success"] = "Reservation saved. Pay at site.";
        return RedirectToAction(nameof(Summary), new { id });
    }
}

