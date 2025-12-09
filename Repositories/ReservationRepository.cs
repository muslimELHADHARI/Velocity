using Microsoft.EntityFrameworkCore;
using Velocity.Data;
using Velocity.Models;

namespace Velocity.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await _context.Reservations
            .Include(r => r.Vehicle)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetForUserAsync(string userId)
    {
        return await _context.Reservations
            .Include(r => r.Vehicle)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public Task<Reservation?> GetByIdAsync(int id)
    {
        return _context.Reservations
            .Include(r => r.Vehicle)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AddAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
    }

    public Task UpdateAsync(Reservation reservation)
    {
        _context.Reservations.Update(reservation);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var reservation = await GetByIdAsync(id);
        if (reservation is not null)
        {
            _context.Reservations.Remove(reservation);
        }
    }

    public Task<bool> HasOverlapAsync(int vehicleId, DateTime start, DateTime end, int? ignoreReservationId = null)
    {
        return _context.Reservations.AnyAsync(r =>
            r.VehicleId == vehicleId &&
            (ignoreReservationId == null || r.Id != ignoreReservationId) &&
            r.ReservationStatus != Models.Enums.ReservationStatus.Cancelled &&
            start < r.EndDateTime && end > r.StartDateTime);
    }

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}

