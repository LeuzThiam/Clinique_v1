using System.Net.Http.Json;
using System.Text.Json;
using MaBoutique.Integration.Common;
using MaBoutique.Session;

namespace MaBoutique.Integration.Payments;

public class PaymentGatewayService : IPaymentGatewayService
{
    private readonly IApiGatewayClientFactory _clientFactory;
    private readonly IUserSessionTokenStore _tokenStore;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public PaymentGatewayService(IApiGatewayClientFactory clientFactory, IUserSessionTokenStore tokenStore)
    {
        _clientFactory = clientFactory;
        _tokenStore = tokenStore;
    }

    public async Task<string?> GetPublishableKeyAsync(CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync("/api/paiement/public-key", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return null;

        var dto = await response.Content.ReadFromJsonAsync<PublicKeyDto>(_jsonOptions, cancellationToken);
        return dto?.Key;
    }

    public async Task<PaymentIntentWebResult?> CreatePaymentIntentAsync(long amount, CancellationToken cancellationToken = default)
    {
        var token = _tokenStore.GetJwtToken();
        if (string.IsNullOrWhiteSpace(token))
            return null;

        var client = _clientFactory.CreateClient(token);
        var response = await client.PostAsJsonAsync("/api/paiement/payment-intent", new { amount }, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<PaymentIntentWebResult>(_jsonOptions, cancellationToken);
    }

    private sealed class PublicKeyDto
    {
        public string? Key { get; set; }
    }
}
