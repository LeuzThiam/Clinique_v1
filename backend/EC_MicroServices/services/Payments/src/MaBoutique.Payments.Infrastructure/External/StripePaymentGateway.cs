using MaBoutique.Payments.Application.Abstractions;
using MaBoutique.Payments.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Stripe;

namespace MaBoutique.Payments.Infrastructure.External;

public class StripePaymentGateway : IStripePaymentGateway
{
    private readonly StripeSettings _stripeSettings;

    public StripePaymentGateway(IOptions<StripeSettings> stripeSettings)
    {
        _stripeSettings = stripeSettings.Value;

        if (string.IsNullOrWhiteSpace(_stripeSettings.SecretKey) || _stripeSettings.SecretKey.StartsWith("CHANGE_ME_", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Stripe:SecretKey n'est pas configuree (placeholder detecte). Utilisez User Secrets ou variables d'environnement.");

        if (string.IsNullOrWhiteSpace(_stripeSettings.PublishableKey) || _stripeSettings.PublishableKey.StartsWith("CHANGE_ME_", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Stripe:PublishableKey n'est pas configuree (placeholder detecte). Utilisez User Secrets ou variables d'environnement.");

        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
    }

    public Task<(string ClientSecret, long Amount, string Currency)> CreatePaymentIntentAsync(long amount, CancellationToken cancellationToken = default)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = amount,
            Currency = "eur",
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            }
        };

        var service = new PaymentIntentService();
        var intent = service.Create(options);

        return Task.FromResult((intent.ClientSecret ?? string.Empty, intent.Amount, intent.Currency ?? string.Empty));
    }

    public string GetPublishableKey() => _stripeSettings.PublishableKey;
}
