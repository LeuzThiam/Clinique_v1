using EC_User_Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace EC_User_Service.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> _users =
        [
            new User
            {
                Id = 1,
                Prenom = "Modou",
                Nom = "Thiam",
                Email = "modou@example.com",
                MotDePasse = "123456",
                Role = "Client"
            },
            new User
            {
                Id = 2,
                Prenom = "Fatou",
                Nom = "Diop",
                Email = "fatou@example.com",
                MotDePasse = "123456",
                Role = "Vendeur"
            }
        ];

        [HttpGet("{id:int}")]
        public IActionResult GetUserById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpGet("email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            var user = _users.FirstOrDefault(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (_users.Any(u => string.Equals(u.Email, user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return Conflict(new { message = "Cet email est deja utilise." });
            }

            user.Id = _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1;
            _users.Add(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _users.FirstOrDefault(u =>
                string.Equals(u.Email, request.Email, StringComparison.OrdinalIgnoreCase) &&
                u.MotDePasse == request.MotDePasse);

            return user is null ? Unauthorized() : Ok(user);
        }

        [HttpGet("sellers/{id:int}")]
        public IActionResult GetSellerById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id && string.Equals(u.Role, "Vendeur", StringComparison.OrdinalIgnoreCase));
            if (user is null)
            {
                return NotFound();
            }

            return Ok(new Seller
            {
                Id = user.Id,
                Nom = $"{user.Prenom} {user.Nom}".Trim(),
                Email = user.Email,
                Actif = true
            });
        }
    }
}
