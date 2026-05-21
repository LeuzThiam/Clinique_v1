using System.Net.Http.Headers;

namespace MaBoutique.Integration.Common;

public class ApiGatewayClientFactory : IApiGatewayClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiGatewayClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public HttpClient CreateClient(string? bearerToken = null)
    {
        var client = _httpClientFactory.CreateClient("ApiGateway");

        if (!string.IsNullOrWhiteSpace(bearerToken))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }

        return client;
    }
}
