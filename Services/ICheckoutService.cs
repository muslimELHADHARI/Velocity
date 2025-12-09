using Velocity.ViewModels;

namespace Velocity.Services;

public interface ICheckoutService
{
    Task<CheckoutViewModel?> BuildSummaryAsync(int reservationId);
    Task<bool> ProcessPaymentAsync(int reservationId, string? cardHolder, string? cardNumber);
}

