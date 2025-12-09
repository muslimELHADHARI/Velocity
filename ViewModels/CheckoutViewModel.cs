using Velocity.Models.Enums;

namespace Velocity.ViewModels;

public class CheckoutViewModel
{
    public int ReservationId { get; set; }
    public int VehicleId { get; set; }
    public string VehicleName { get; set; } = string.Empty;
    public VehicleType VehicleType { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public int DurationHours { get; set; }
    public decimal PricePerHour { get; set; }
    public decimal TotalAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public ReservationStatus ReservationStatus { get; set; }
}

