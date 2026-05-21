using System.Net.Http.Json;
using System.Text.Json;
using MaBoutique.Integration.Common;
using MaBoutique.Models;

namespace MaBoutique.Integration.Products;

public class ProductCatalogGatewayService : IProductCatalogGatewayService
{
    private readonly IApiGatewayClientFactory _clientFactory;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ProductCatalogGatewayService(IApiGatewayClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<List<Produit>> GetCatalogAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("/api/produits", cancellationToken);
            if (!response.IsSuccessStatusCode)
                return new List<Produit>();

            var produits = await response.Content.ReadFromJsonAsync<List<ProduitApiListItemDto>>(_jsonOptions, cancellationToken);
            return produits?.Select(p => new Produit
            {
                Id = p.Id,
                Nom = p.Nom ?? string.Empty,
                Prix = p.Prix,
                CategorieNom = p.CategorieNom ?? string.Empty,
                UrlImage = p.UrlImage ?? string.Empty,
                Description = "Description disponible sur la page de details."
            }).ToList() ?? new List<Produit>();
        }
        catch
        {
            return new List<Produit>();
        }
    }

    public async Task<Produit?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"/api/produits/{id}", cancellationToken);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Produit>(_jsonOptions, cancellationToken);
        }
        catch
        {
            return null;
        }
    }

    private sealed class ProduitApiListItemDto
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public decimal Prix { get; set; }
        public string? CategorieNom { get; set; }
        public string? UrlImage { get; set; }
    }
}
