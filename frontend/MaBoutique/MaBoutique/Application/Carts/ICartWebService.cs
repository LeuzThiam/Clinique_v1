using MaBoutique.Models;

namespace MaBoutique.Application.Carts;

public interface ICartWebService
{
    Task<Panier?> GetOrCreatePanierAsync(int utilisateurId, CancellationToken cancellationToken = default);
    Task AddArticleAsync(int utilisateurId, int idProduit, int quantite = 1, CancellationToken cancellationToken = default);
    Task RemoveArticleAsync(int utilisateurId, int idProduit, CancellationToken cancellationToken = default);
}
