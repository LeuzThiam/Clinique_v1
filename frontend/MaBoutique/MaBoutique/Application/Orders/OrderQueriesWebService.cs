using MaBoutique.Models;
using Microsoft.EntityFrameworkCore;

namespace MaBoutique.Application.Orders;

public class OrderQueriesWebService : IOrderQueriesWebService
{
    private readonly ApplicationDbContext _context;

    public OrderQueriesWebService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Commande>> GetHistoriqueAsync(int utilisateurId, CancellationToken cancellationToken = default)
    {
        return _context.Commandes
            .Include(c => c.ArticlesCommandes)
                .ThenInclude(ac => ac.Produit)
            .Where(c => c.UtilisateurId == utilisateurId)
            .OrderByDescending(c => c.DateCommande)
            .ToListAsync(cancellationToken);
    }

    public Task<Commande?> GetDetailsAsync(int commandeId, int utilisateurId, CancellationToken cancellationToken = default)
    {
        return _context.Commandes
            .Include(c => c.ArticlesCommandes)
                .ThenInclude(a => a.Produit)
            .FirstOrDefaultAsync(c => c.Id == commandeId && c.UtilisateurId == utilisateurId, cancellationToken);
    }
}
