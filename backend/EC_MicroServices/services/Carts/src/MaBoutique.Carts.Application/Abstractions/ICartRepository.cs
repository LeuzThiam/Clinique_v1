using MaBoutique.Carts.Domain.Entities;

namespace MaBoutique.Carts.Application.Abstractions;

public interface ICartRepository
{
    Task<Panier?> GetByUserIdAsync(int utilisateurId, CancellationToken cancellationToken = default);
    Task AddAsync(Panier panier, CancellationToken cancellationToken = default);
    void RemoveArticle(ArticlePanier article);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
