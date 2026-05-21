using MaBoutique.Payments.Application.Abstractions;
using MaBoutique.Payments.Application.Dtos;

namespace MaBoutique.Payments.Application.UseCases;

public class PaymentsApplicationService : IPaymentsApplicationService
{
    private readonly IStripePaymentGateway _stripeGateway;

    public PaymentsApplicationService(IStripePaymentGateway stripeGateway)
    {
        _stripeGateway = stripeGateway;
    }

    public async Task<PaymentIntentResultDto> CreatePaymentIntentAsync(PaiementRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Amount <= 0)
            throw new ArgumentException("Le montant doit être supérieur à 0.", nameof(request.Amount));

        var result = await _stripeGateway.CreatePaymentIntentAsync(request.Amount, cancellationToken);
        return new PaymentIntentResultDto
        {
            ClientSecret = result.ClientSecret,
            Amount = result.Amount,
            Currency = result.Currency
        };
    }

    public string GetPublishableKey() => _stripeGateway.GetPublishableKey();
}
