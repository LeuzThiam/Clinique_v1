using EC_Payment_Service.Models;

namespace EC_Payment_Service.Services;

public interface IStripePaymentService
{
    string GetPublishableKey();
    bool IsConfigured();
    Task<PaymentIntentResult> CreatePaymentIntentAsync(long amount, CancellationToken cancellationToken = default);
}
