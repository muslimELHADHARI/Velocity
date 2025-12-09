using System.ComponentModel.DataAnnotations;
using Velocity.Models.Enums;

namespace Velocity.ViewModels;

public class ReservationCreateViewModel
{
    [Required]
    public int VehicleId { get; set; }
    public string VehicleName { get; set; } = string.Empty;
    public string VehicleBrand { get; set; } = string.Empty;
    public VehicleType VehicleType { get; set; }
    public decimal PricePerHour { get; set; }

    [Required]
    public DateTime StartDateTime { get; set; } = DateTime.UtcNow.AddHours(1);

    [Range(1, 168)]
    public int DurationHours { get; set; } = 2;

    [Required]
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Online;
}

