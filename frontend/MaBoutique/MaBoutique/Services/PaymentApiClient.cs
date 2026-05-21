using System.Net.Http.Json;
using System.Text.Json;
using MaBoutique.Services.ApiModels;

namespace MaBoutique.Services;

public class PaymentApiClient : IPaymentApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;

    public PaymentApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaymentPublicKeyApiModel?> GetPublishableKeyAsync(CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync("api/payments/public-key", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<PaymentPublicKeyApiModel>(JsonOptions, cancellationToken);
    }

    public async Task<PaymentIntentApiModel?> CreatePaymentIntentAsync(long amount, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync("api/payments/payment-intent", new { amount }, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<PaymentIntentApiModel>(JsonOptions, cancellationToken);
    }
}
