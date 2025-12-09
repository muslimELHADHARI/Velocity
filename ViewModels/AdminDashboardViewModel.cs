using Velocity.Models;
using Velocity.Models.Enums;

namespace Velocity.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalUsers { get; set; }
    public int TotalVehicles { get; set; }
    public int BicycleReservations { get; set; }
    public int MotorcycleReservations { get; set; }
    public decimal OnlineRevenue { get; set; }
    public IEnumerable<Reservation> RecentReservations { get; set; } = Enumerable.Empty<Reservation>();
    public IEnumerable<Vehicle> RecentVehicles { get; set; } = Enumerable.Empty<Vehicle>();
}

