using System.Net.Http.Json;
using MaBoutique.Auth.Application.Abstractions;
using MaBoutique.Auth.Application.Dtos;

namespace MaBoutique.Auth.Infrastructure.External;

public class UsersHttpClient : IUsersClient
{
    private readonly HttpClient _httpClient;

    public UsersHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UtilisateurDTO?> VerifyPasswordAsync(string email, string motDePasse, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/utilisateurs/verify-password", new { Email = email, MotDePasse = motDePasse }, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UtilisateurDTO>(cancellationToken: cancellationToken);
    }
}
