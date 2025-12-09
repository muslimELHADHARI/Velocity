using Velocity.Models;
using Velocity.Models.Enums;

namespace Velocity.Repositories;

public interface IVehicleRepository
{
    Task<IEnumerable<Vehicle>> GetAllAsync(string? search, VehicleType? type, string? brand);
    Task<Vehicle?> GetByIdAsync(int id);
    Task AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}

