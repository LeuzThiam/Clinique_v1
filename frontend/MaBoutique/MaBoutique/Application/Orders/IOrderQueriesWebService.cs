using MaBoutique.Models;

namespace MaBoutique.Application.Orders;

public interface IOrderQueriesWebService
{
    Task<List<Commande>> GetHistoriqueAsync(int utilisateurId, CancellationToken cancellationToken = default);
    Task<Commande?> GetDetailsAsync(int commandeId, int utilisateurId, CancellationToken cancellationToken = default);
}
