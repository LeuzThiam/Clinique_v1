using MaBoutique.Auth.Application.Dtos;

namespace MaBoutique.Auth.Application.Abstractions;

public interface IJwtTokenGenerator
{
    AuthResponseDTO CreateToken(UtilisateurDTO utilisateur);
}
