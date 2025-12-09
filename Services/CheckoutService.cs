using Velocity.Models.Enums;
using Velocity.Repositories;
using Velocity.ViewModels;

namespace Velocity.Services;

public class CheckoutService : ICheckoutService
{
    private readonly IReservationRepository _reservationRepository;

    public CheckoutService(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<CheckoutViewModel?> BuildSummaryAsync(int reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation is null || reservation.Vehicle is null) return null;

        var duration = (int)(reservation.EndDateTime - reservation.StartDateTime).TotalHours;

        return new CheckoutViewModel
        {
            ReservationId = reservation.Id,
            VehicleId = reservation.VehicleId,
            VehicleName = reservation.Vehicle.Name,
            VehicleType = reservation.Vehicle.Type,
            StartDateTime = reservation.StartDateTime,
            EndDateTime = reservation.EndDateTime,
            DurationHours = duration,
            PricePerHour = reservation.Vehicle.PricePerHour,
            TotalAmount = reservation.TotalAmount,
            PaymentMethod = reservation.PaymentMethod,
            PaymentStatus = reservation.PaymentStatus,
            ReservationStatus = reservation.ReservationStatus
        };
    }

    public async Task<bool> ProcessPaymentAsync(int reservationId, string? cardHolder, string? cardNumber)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation is null) return false;

        // Dummy payment validation
        var isValid = !string.IsNullOrWhiteSpace(cardHolder) && !string.IsNullOrWhiteSpace(cardNumber);
        reservation.PaymentStatus = isValid ? PaymentStatus.Paid : PaymentStatus.Failed;
        reservation.ReservationStatus = isValid ? ReservationStatus.Confirmed : ReservationStatus.Pending;
        reservation.PaymentReference = isValid ? $"VIRTUAL-{Guid.NewGuid().ToString()[..8]}" : null;

        await _reservationRepository.UpdateAsync(reservation);
        await _reservationRepository.SaveChangesAsync();

        return isValid;
    }
}

