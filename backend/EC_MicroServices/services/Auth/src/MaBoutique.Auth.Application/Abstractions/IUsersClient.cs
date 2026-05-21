using MaBoutique.Auth.Application.Dtos;

namespace MaBoutique.Auth.Application.Abstractions;

public interface IUsersClient
{
    Task<UtilisateurDTO?> VerifyPasswordAsync(string email, string motDePasse, CancellationToken cancellationToken = default);
}
