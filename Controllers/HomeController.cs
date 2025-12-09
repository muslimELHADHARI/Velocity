using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Velocity.Models;
using Velocity.Services;

namespace Velocity.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IVehicleService _vehicleService;

    public HomeController(ILogger<HomeController> logger, IVehicleService vehicleService)
    {
        _logger = logger;
        _vehicleService = vehicleService;
    }

    public async Task<IActionResult> Index()
    {
        var catalog = await _vehicleService.GetCatalogAsync(null, null, null);
        catalog.Vehicles = catalog.Vehicles.Take(6);
        return View(catalog);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
