using MaBoutique.Payments.Application.Dtos;

namespace MaBoutique.Payments.Application.Abstractions;

public interface IPaymentsApplicationService
{
    Task<PaymentIntentResultDto> CreatePaymentIntentAsync(PaiementRequest request, CancellationToken cancellationToken = default);
    string GetPublishableKey();
}
