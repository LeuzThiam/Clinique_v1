using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using MaBoutique.Integration.Common;
using MaBoutique.Models;
using Microsoft.AspNetCore.Http;

namespace MaBoutique.Integration.Auth;

public class AuthGatewayService : IAuthGatewayService
{
    private readonly IApiGatewayClientFactory _clientFactory;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public AuthGatewayService(IApiGatewayClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<UserRegistrationGatewayStatus> RegisterAsync(Utilisateur utilisateur, CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.CreateClient();

        var payload = new
        {
            nom = utilisateur.Nom,
            prenom = utilisateur.Prenom,
            email = utilisateur.Email,
            motDePasse = utilisateur.MotDePasse,
            adresse = utilisateur.Adresse,
            ville = utilisateur.Ville,
            codePostal = utilisateur.CodePostal,
            province = utilisateur.Province,
            pays = utilisateur.Pays,
            role = utilisateur.Role.ToString()
        };

        var response = await client.PostAsJsonAsync("/api/utilisateurs", payload, cancellationToken);
        if (response.IsSuccessStatusCode)
            return UserRegistrationGatewayStatus.Success;

        if ((int)response.StatusCode == StatusCodes.Status409Conflict)
            return UserRegistrationGatewayStatus.EmailAlreadyExists;

        return UserRegistrationGatewayStatus.Failed;
    }

    public async Task<GatewayAuthenticationResult?> AuthenticateAsync(string email, string motDePasse, CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.CreateClient();

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new { email, motDePasse }, cancellationToken);
        if (!loginResponse.IsSuccessStatusCode)
            return null;

        var auth = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>(_jsonOptions, cancellationToken);
        if (auth?.Token == null)
            return null;

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(auth.Token);

        var idClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "nameid")?.Value;
        var emailClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == ClaimTypes.Email || c.Type == "unique_name")?.Value;
        var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value;

        if (!int.TryParse(idClaim, out var userId))
            return null;

        var securedClient = _clientFactory.CreateClient(auth.Token);
        var userResponse = await securedClient.GetAsync($"/api/utilisateurs/{userId}", cancellationToken);

        Utilisateur utilisateur;
        if (userResponse.IsSuccessStatusCode)
        {
            var userDto = await userResponse.Content.ReadFromJsonAsync<UtilisateurApiDto>(_jsonOptions, cancellationToken);
            utilisateur = new Utilisateur
            {
                Id = userDto?.Id ?? userId,
                Prenom = userDto?.Prenom ?? string.Empty,
                Nom = userDto?.Nom ?? string.Empty,
                Email = userDto?.Email ?? emailClaim ?? email,
                Role = ParseRole(userDto?.Role, roleClaim),
                MotDePasse = string.Empty,
                Adresse = string.Empty,
                Ville = string.Empty,
                CodePostal = string.Empty,
                Province = string.Empty,
                Pays = string.Empty,
                Telephone = string.Empty
            };
        }
        else
        {
            utilisateur = new Utilisateur
            {
                Id = userId,
                Prenom = string.Empty,
                Nom = string.Empty,
                Email = emailClaim ?? email,
                Role = ParseRole(roleClaim, null),
                MotDePasse = string.Empty,
                Adresse = string.Empty,
                Ville = string.Empty,
                CodePostal = string.Empty,
                Province = string.Empty,
                Pays = string.Empty,
                Telephone = string.Empty
            };
        }

        return new GatewayAuthenticationResult
        {
            Utilisateur = utilisateur,
            JwtToken = auth.Token,
            JwtExpiration = auth.Expiration
        };
    }

    private static RoleUtilisateur ParseRole(string? primaryRole, string? fallbackRole)
    {
        var candidate = string.IsNullOrWhiteSpace(primaryRole) ? fallbackRole : primaryRole;
        return Enum.TryParse<RoleUtilisateur>(candidate, true, out var parsed)
            ? parsed
            : RoleUtilisateur.Client;
    }

    private sealed class AuthResponseDto
    {
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
    }

    private sealed class UtilisateurApiDto
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
