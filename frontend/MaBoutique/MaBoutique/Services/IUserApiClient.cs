using MaBoutique.Models;

namespace MaBoutique.Services
{
    public interface IUserApiClient
    {
        Task<Utilisateur?> RegisterAsync(Utilisateur utilisateur);
        Task<Utilisateur?> LoginAsync(string email, string motDePasse);
        Task<Utilisateur?> GetByIdAsync(int id);
    }
}
