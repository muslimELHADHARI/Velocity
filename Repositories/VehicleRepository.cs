using Microsoft.EntityFrameworkCore;
using Velocity.Data;
using Velocity.Models;
using Velocity.Models.Enums;

namespace Velocity.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationDbContext _context;

    public VehicleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync(string? search, VehicleType? type, string? brand)
    {
        var query = _context.Vehicles.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(v => v.Name.Contains(search) || v.Brand.Contains(search));
        }

        if (type.HasValue)
        {
            query = query.Where(v => v.Type == type);
        }

        if (!string.IsNullOrWhiteSpace(brand))
        {
            query = query.Where(v => v.Brand == brand);
        }

        return await query.OrderByDescending(v => v.CreatedAt).ToListAsync();
    }

    public Task<Vehicle?> GetByIdAsync(int id) =>
        _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);

    public async Task AddAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
    }

    public Task UpdateAsync(Vehicle vehicle)
    {
        _context.Vehicles.Update(vehicle);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var vehicle = await GetByIdAsync(id);
        if (vehicle is not null)
        {
            _context.Vehicles.Remove(vehicle);
        }
    }

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}

