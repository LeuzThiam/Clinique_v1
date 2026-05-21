using MaBoutique.Services.ApiModels;

namespace MaBoutique.Services;

public interface IPaymentApiClient
{
    Task<PaymentPublicKeyApiModel?> GetPublishableKeyAsync(CancellationToken cancellationToken = default);
    Task<PaymentIntentApiModel?> CreatePaymentIntentAsync(long amount, CancellationToken cancellationToken = default);
}
