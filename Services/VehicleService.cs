using Velocity.Models;
using Velocity.Models.Enums;
using Velocity.Repositories;
using Velocity.ViewModels;

namespace Velocity.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<VehicleListViewModel> GetCatalogAsync(string? search, VehicleType? type, string? brand)
    {
        var vehicles = await _vehicleRepository.GetAllAsync(search, type, brand);
        return new VehicleListViewModel
        {
            Vehicles = vehicles,
            Search = search,
            Type = type,
            Brand = brand
        };
    }

    public Task<Vehicle?> GetByIdAsync(int id) => _vehicleRepository.GetByIdAsync(id);

    public async Task AddAsync(VehicleFormViewModel model)
    {
        var vehicle = new Vehicle
        {
            Name = model.Name,
            Type = model.Type,
            Brand = model.Brand,
            Description = model.Description,
            EngineSize = model.EngineSize,
            PricePerHour = model.PricePerHour,
            ImageUrl = model.ImageUrl,
            AvailabilityStatus = model.AvailabilityStatus
        };

        await _vehicleRepository.AddAsync(vehicle);
        await _vehicleRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(VehicleFormViewModel model)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(model.Id!.Value);
        if (vehicle is null) return;

        vehicle.Name = model.Name;
        vehicle.Type = model.Type;
        vehicle.Brand = model.Brand;
        vehicle.Description = model.Description;
        vehicle.EngineSize = model.EngineSize;
        vehicle.PricePerHour = model.PricePerHour;
        vehicle.ImageUrl = model.ImageUrl;
        vehicle.AvailabilityStatus = model.AvailabilityStatus;

        await _vehicleRepository.UpdateAsync(vehicle);
        await _vehicleRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _vehicleRepository.DeleteAsync(id);
        await _vehicleRepository.SaveChangesAsync();
    }
}

