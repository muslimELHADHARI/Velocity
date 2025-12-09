using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Velocity.Models.Enums;
using Velocity.Services;
using Velocity.ViewModels;

namespace Velocity.Controllers;

public class VehiclesController : Controller
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(string? search, VehicleType? type, string? brand)
    {
        var model = await _vehicleService.GetCatalogAsync(search, type, brand);
        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);
        if (vehicle is null) return NotFound();
        return View(vehicle);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View(new VehicleFormViewModel());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VehicleFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await _vehicleService.AddAsync(model);
        TempData["Success"] = "Vehicle created.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);
        if (vehicle is null) return NotFound();

        var vm = new VehicleFormViewModel
        {
            Id = vehicle.Id,
            Name = vehicle.Name,
            Type = vehicle.Type,
            Brand = vehicle.Brand,
            Description = vehicle.Description,
            EngineSize = vehicle.EngineSize,
            PricePerHour = vehicle.PricePerHour,
            ImageUrl = vehicle.ImageUrl,
            AvailabilityStatus = vehicle.AvailabilityStatus
        };

        return View(vm);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, VehicleFormViewModel model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);
        await _vehicleService.UpdateAsync(model);
        TempData["Success"] = "Vehicle updated.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);
        if (vehicle is null) return NotFound();
        return View(vehicle);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _vehicleService.DeleteAsync(id);
        TempData["Success"] = "Vehicle deleted.";
        return RedirectToAction(nameof(Index));
    }
}

