using MaBoutique.Auth.Application.Abstractions;
using MaBoutique.Auth.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MaBoutique.Auth.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthApplicationService _authService;

        public AuthController(IAuthApplicationService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ConnexionDTO dto, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(dto, cancellationToken);
            if (result is null)
                return Unauthorized("Email ou mot de passe incorrect.");

            return Ok(result);
        }
    }
}
