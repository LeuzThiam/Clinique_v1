using MaBoutique.Models;

namespace MaBoutique.Services
{
    public interface IProductApiClient
    {
        Task<IReadOnlyList<Produit>> GetProduitsAsync(string? search = null);
        Task<Produit?> GetProduitAsync(int id);
        Task<Produit?> CreateProduitAsync(Produit produit);
        Task<bool> UpdateProduitAsync(Produit produit);
        Task<bool> DeleteProduitAsync(int id);
    }
}
