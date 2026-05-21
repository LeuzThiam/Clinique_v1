using MaBoutique.Models;

namespace MaBoutique.Integration.Products;

public interface IProductCatalogGatewayService
{
    Task<List<Produit>> GetCatalogAsync(CancellationToken cancellationToken = default);
    Task<Produit?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
