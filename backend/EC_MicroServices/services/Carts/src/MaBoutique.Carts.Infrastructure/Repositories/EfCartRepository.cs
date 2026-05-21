using MaBoutique.Carts.Application.Abstractions;
using MaBoutique.Carts.Domain.Entities;
using MaBoutique.Carts.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MaBoutique.Carts.Infrastructure.Repositories;

public class EfCartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;

    public EfCartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Panier?> GetByUserIdAsync(int utilisateurId, CancellationToken cancellationToken = default)
    {
        return _context.Paniers
            .Include(p => p.ArticlesPaniers)
            .FirstOrDefaultAsync(p => p.IdUtilisateur == utilisateurId, cancellationToken);
    }

    public async Task AddAsync(Panier panier, CancellationToken cancellationToken = default)
    {
        await _context.Paniers.AddAsync(panier, cancellationToken);
    }

    public void RemoveArticle(ArticlePanier article)
    {
        _context.ArticlesPaniers.Remove(article);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
