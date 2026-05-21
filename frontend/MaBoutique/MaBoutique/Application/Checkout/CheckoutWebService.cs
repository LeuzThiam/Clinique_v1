using MaBoutique.Models;
using Microsoft.EntityFrameworkCore;

namespace MaBoutique.Application.Checkout;

public class CheckoutWebService : ICheckoutWebService
{
    private readonly ApplicationDbContext _context;

    public CheckoutWebService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, int? CommandeId)> FinaliserCommandePayeAsync(int utilisateurId, CancellationToken cancellationToken = default)
    {
        var panier = await _context.Paniers
            .Include(p => p.ArticlesPaniers)
            .ThenInclude(a => a.Produit)
            .FirstOrDefaultAsync(p => p.IdUtilisateur == utilisateurId, cancellationToken);

        if (panier == null || !panier.ArticlesPaniers.Any())
            return (false, null);

        var total = panier.ArticlesPaniers.Sum(a => (a.Produit?.Prix ?? 0m) * a.Quantite);

        var commande = new Commande
        {
            UtilisateurId = utilisateurId,
            DateCommande = DateTime.Now,
            EstPayee = true,
            Total = total
        };
        _context.Commandes.Add(commande);
        await _context.SaveChangesAsync(cancellationToken);

        foreach (var item in panier.ArticlesPaniers)
        {
            _context.ArticlesCommandes.Add(new ArticleCommande
            {
                CommandeId = commande.Id,
                ProduitId = item.ProduitId,
                Quantite = item.Quantite,
                PrixUnitaire = item.Produit?.Prix ?? 0m
            });
        }
        await _context.SaveChangesAsync(cancellationToken);

        var facture = new Facture
        {
            NumeroFacture = $"F-{DateTime.Now:yyyyMMdd}-{commande.Id}",
            DateFacturation = DateTime.Now,
            MontantTotal = total,
            CommandeId = commande.Id,
            UtilisateurId = utilisateurId,
            ArticlesFactures = await _context.ArticlesCommandes.Where(ac => ac.CommandeId == commande.Id).ToListAsync(cancellationToken)
        };
        _context.Factures.Add(facture);

        _context.ArticlesPaniers.RemoveRange(panier.ArticlesPaniers);
        await _context.SaveChangesAsync(cancellationToken);

        return (true, commande.Id);
    }
}
