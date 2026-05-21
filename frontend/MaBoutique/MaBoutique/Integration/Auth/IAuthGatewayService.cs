using MaBoutique.Models;

namespace MaBoutique.Integration.Auth;

public sealed class GatewayAuthenticationResult
{
    public required Utilisateur Utilisateur { get; init; }
    public required string JwtToken { get; init; }
    public DateTime JwtExpiration { get; init; }
}

public enum UserRegistrationGatewayStatus
{
    Success = 0,
    EmailAlreadyExists = 1,
    Failed = 2
}

public interface IAuthGatewayService
{
    Task<UserRegistrationGatewayStatus> RegisterAsync(Utilisateur utilisateur, CancellationToken cancellationToken = default);
    Task<GatewayAuthenticationResult?> AuthenticateAsync(string email, string motDePasse, CancellationToken cancellationToken = default);
}
