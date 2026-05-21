namespace MaBoutique.Integration.Common;

public interface IApiGatewayClientFactory
{
    HttpClient CreateClient(string? bearerToken = null);
}
