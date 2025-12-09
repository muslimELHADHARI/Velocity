using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Velocity.Models.Enums;

namespace Velocity.Models;

public class Vehicle
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public VehicleType Type { get; set; }

    [Required, MaxLength(120)]
    public string Brand { get; set; } = string.Empty;

    [MaxLength(1024)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? EngineSize { get; set; }

    [Column(TypeName = "decimal(10,2)"), Range(0, 10000)]
    public decimal PricePerHour { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public AvailabilityStatus AvailabilityStatus { get; set; } = AvailabilityStatus.Available;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}

