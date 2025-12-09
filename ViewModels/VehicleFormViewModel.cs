using System.ComponentModel.DataAnnotations;
using Velocity.Models.Enums;

namespace Velocity.ViewModels;

public class VehicleFormViewModel
{
    public int? Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public VehicleType Type { get; set; }

    [Required, MaxLength(120)]
    public string Brand { get; set; } = string.Empty;

    [MaxLength(1024)]
    public string? Description { get; set; }

    [Range(0, 2000)]
    public decimal? EngineSize { get; set; }

    [Range(0, 10000)]
    public decimal PricePerHour { get; set; }

    [Url]
    public string? ImageUrl { get; set; }

    public AvailabilityStatus AvailabilityStatus { get; set; } = AvailabilityStatus.Available;
}

