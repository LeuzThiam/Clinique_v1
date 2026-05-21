namespace MaBoutique.Payments.Application.Abstractions;

public interface IStripePaymentGateway
{
    Task<(string ClientSecret, long Amount, string Currency)> CreatePaymentIntentAsync(long amount, CancellationToken cancellationToken = default);
    string GetPublishableKey();
}
