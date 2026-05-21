using EC_Payment_Service.Models;
using Microsoft.Extensions.Options;
using Stripe;

namespace EC_Payment_Service.Services;

public class StripePaymentService : IStripePaymentService
{
    private readonly StripeSettings _settings;

    public StripePaymentService(IOptions<StripeSettings> options)
    {
        _settings = options.Value;
    }

    public string GetPublishableKey() => _settings.PublishableKey;

    public bool IsConfigured() => _settings.IsConfigured();

    public async Task<PaymentIntentResult> CreatePaymentIntentAsync(long amount, CancellationToken cancellationToken = default)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Le montant doit etre superieur a 0.", nameof(amount));
        }

        if (!_settings.IsConfigured())
        {
            throw new InvalidOperationException("Stripe n'est pas configure.");
        }

        StripeConfiguration.ApiKey = _settings.SecretKey;

        var service = new PaymentIntentService();
        var intent = await service.CreateAsync(new PaymentIntentCreateOptions
        {
            Amount = amount,
            Currency = "cad",
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            }
        }, cancellationToken: cancellationToken);

        return new PaymentIntentResult
        {
            ClientSecret = intent.ClientSecret ?? string.Empty,
            Amount = intent.Amount,
            Currency = intent.Currency ?? "cad"
        };
    }
}
