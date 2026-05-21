using MaBoutique.Models;
using Microsoft.EntityFrameworkCore;

namespace MaBoutique.Application.Carts;

public class CartWebService : ICartWebService
{
    private readonly ApplicationDbContext _context;

    public CartWebService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Panier?> GetOrCreatePanierAsync(int utilisateurId, CancellationToken cancellationToken = default)
    {
        var panier = await _context.Paniers
            .Include(p => p.ArticlesPaniers)
                .ThenInclude(ap => ap.Produit)
            .FirstOrDefaultAsync(p => p.IdUtilisateur == utilisateurId, cancellationToken);

        if (panier != null)
            return panier;

        panier = new Panier
        {
            IdUtilisateur = utilisateurId,
            ArticlesPaniers = new List<ArticlePanier>()
        };

        _context.Paniers.Add(panier);
        await _context.SaveChangesAsync(cancellationToken);
        return panier;
    }

    public async Task AddArticleAsync(int utilisateurId, int idProduit, int quantite = 1, CancellationToken cancellationToken = default)
    {
        var produit = await _context.Produits.FindAsync(new object[] { idProduit }, cancellationToken);
        if (produit == null)
            throw new KeyNotFoundException("Produit introuvable.");

        var panier = await GetOrCreatePanierAsync(utilisateurId, cancellationToken) ?? throw new InvalidOperationException();
        var article = panier.ArticlesPaniers.FirstOrDefault(a => a.ProduitId == idProduit);

        if (article != null)
        {
            article.Quantite += quantite;
            _context.ArticlesPaniers.Update(article);
        }
        else
        {
            panier.ArticlesPaniers.Add(new ArticlePanier
            {
                ProduitId = idProduit,
                Quantite = quantite,
                PanierId = panier.Id
            });
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveArticleAsync(int utilisateurId, int idProduit, CancellationToken cancellationToken = default)
    {
        var panier = await GetOrCreatePanierAsync(utilisateurId, cancellationToken);
        if (panier == null)
            return;

        var article = panier.ArticlesPaniers.FirstOrDefault(a => a.ProduitId == idProduit);
        if (article == null)
            return;

        _context.ArticlesPaniers.Remove(article);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
