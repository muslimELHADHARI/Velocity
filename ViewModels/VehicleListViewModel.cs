using Velocity.Models;
using Velocity.Models.Enums;

namespace Velocity.ViewModels;

public class VehicleListViewModel
{
    public IEnumerable<Vehicle> Vehicles { get; set; } = Enumerable.Empty<Vehicle>();
    public string? Search { get; set; }
    public VehicleType? Type { get; set; }
    public string? Brand { get; set; }
}

