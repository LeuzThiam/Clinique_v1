using MaBoutique.Auth.Application.Dtos;

namespace MaBoutique.Auth.Application.Abstractions;

public interface IAuthApplicationService
{
    Task<AuthResponseDTO?> LoginAsync(ConnexionDTO dto, CancellationToken cancellationToken = default);
}
