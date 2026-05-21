using MaBoutique.Auth.Application.Abstractions;
using MaBoutique.Auth.Application.Dtos;

namespace MaBoutique.Auth.Application.UseCases;

public class AuthApplicationService : IAuthApplicationService
{
    private readonly IUsersClient _usersClient;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthApplicationService(IUsersClient usersClient, IJwtTokenGenerator jwtTokenGenerator)
    {
        _usersClient = usersClient;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDTO?> LoginAsync(ConnexionDTO dto, CancellationToken cancellationToken = default)
    {
        var utilisateur = await _usersClient.VerifyPasswordAsync(dto.Email, dto.MotDePasse, cancellationToken);
        if (utilisateur == null)
            return null;

        return _jwtTokenGenerator.CreateToken(utilisateur);
    }
}
