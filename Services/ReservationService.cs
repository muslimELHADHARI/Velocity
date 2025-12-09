using Velocity.Models;
using Velocity.Models.Enums;
using Velocity.Repositories;
using Velocity.ViewModels;

namespace Velocity.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IVehicleRepository _vehicleRepository;

    public ReservationService(IReservationRepository reservationRepository, IVehicleRepository vehicleRepository)
    {
        _reservationRepository = reservationRepository;
        _vehicleRepository = vehicleRepository;
    }

    public async Task<ReservationCreateViewModel?> BuildCreateModelAsync(int vehicleId)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
        if (vehicle is null) return null;

        return new ReservationCreateViewModel
        {
            VehicleId = vehicle.Id,
            VehicleName = vehicle.Name,
            VehicleBrand = vehicle.Brand,
            VehicleType = vehicle.Type,
            PricePerHour = vehicle.PricePerHour,
            StartDateTime = DateTime.UtcNow.AddHours(1),
            DurationHours = 2,
            PaymentMethod = PaymentMethod.Online
        };
    }

    public async Task<(bool Success, string Error, Reservation? Reservation)> CreateAsync(string userId, ReservationCreateViewModel model)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(model.VehicleId);
        if (vehicle is null) return (false, "Vehicle not found.", null);

        var start = model.StartDateTime.ToUniversalTime();
        var end = start.AddHours(model.DurationHours);

        var overlaps = await _reservationRepository.HasOverlapAsync(model.VehicleId, start, end);
        if (overlaps) return (false, "This vehicle is already booked for that time slot.", null);

        var reservation = new Reservation
        {
            VehicleId = vehicle.Id,
            UserId = userId,
            StartDateTime = start,
            EndDateTime = end,
            TotalAmount = vehicle.PricePerHour * model.DurationHours,
            PaymentMethod = model.PaymentMethod,
            PaymentStatus = PaymentStatus.Pending,
            ReservationStatus = ReservationStatus.Pending
        };

        await _reservationRepository.AddAsync(reservation);

        vehicle.AvailabilityStatus = AvailabilityStatus.Reserved;
        await _vehicleRepository.UpdateAsync(vehicle);

        await _reservationRepository.SaveChangesAsync();
        await _vehicleRepository.SaveChangesAsync();

        return (true, string.Empty, reservation);
    }

    public Task<IEnumerable<Reservation>> GetForUserAsync(string userId) =>
        _reservationRepository.GetForUserAsync(userId);

    public Task<IEnumerable<Reservation>> GetAllAsync() =>
        _reservationRepository.GetAllAsync();

    public async Task UpdateStatusAsync(int reservationId, bool markPaid)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation is null) return;

        if (markPaid)
        {
            reservation.PaymentStatus = PaymentStatus.Paid;
            reservation.ReservationStatus = ReservationStatus.Confirmed;
        }
        else
        {
            reservation.PaymentStatus = PaymentStatus.Pending;
            reservation.ReservationStatus = ReservationStatus.Pending;
        }

        await _reservationRepository.UpdateAsync(reservation);
        await _reservationRepository.SaveChangesAsync();
    }
}

