using Velocity.Models;

namespace Velocity.Repositories;

public interface IReservationRepository
{
    Task<IEnumerable<Reservation>> GetAllAsync();
    Task<IEnumerable<Reservation>> GetForUserAsync(string userId);
    Task<Reservation?> GetByIdAsync(int id);
    Task AddAsync(Reservation reservation);
    Task UpdateAsync(Reservation reservation);
    Task DeleteAsync(int id);
    Task<bool> HasOverlapAsync(int vehicleId, DateTime start, DateTime end, int? ignoreReservationId = null);
    Task SaveChangesAsync();
}

