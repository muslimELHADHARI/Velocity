using Velocity.Models;
using Velocity.Models.Enums;
using Velocity.ViewModels;

namespace Velocity.Services;

public interface IVehicleService
{
    Task<VehicleListViewModel> GetCatalogAsync(string? search, VehicleType? type, string? brand);
    Task<Vehicle?> GetByIdAsync(int id);
    Task AddAsync(VehicleFormViewModel model);
    Task UpdateAsync(VehicleFormViewModel model);
    Task DeleteAsync(int id);
}

