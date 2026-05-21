namespace MaBoutique.Integration.Payments;

public sealed class PaymentIntentWebResult
{
    public string ClientSecret { get; set; } = string.Empty;
}

public interface IPaymentGatewayService
{
    Task<string?> GetPublishableKeyAsync(CancellationToken cancellationToken = default);
    Task<PaymentIntentWebResult?> CreatePaymentIntentAsync(long amount, CancellationToken cancellationToken = default);
}
