using Velocity.Models;
using Velocity.ViewModels;

namespace Velocity.Services;

public interface IReservationService
{
    Task<ReservationCreateViewModel?> BuildCreateModelAsync(int vehicleId);
    Task<(bool Success, string Error, Reservation? Reservation)> CreateAsync(string userId, ReservationCreateViewModel model);
    Task<IEnumerable<Reservation>> GetForUserAsync(string userId);
    Task<IEnumerable<Reservation>> GetAllAsync();
    Task UpdateStatusAsync(int reservationId, bool markPaid);
}

