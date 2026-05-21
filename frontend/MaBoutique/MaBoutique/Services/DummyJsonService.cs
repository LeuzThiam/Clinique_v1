using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MaBoutique.Models;

namespace MaBoutique.Services
{
    public class DummyJsonService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _db;

        public DummyJsonService(HttpClient httpClient, ApplicationDbContext db)
        {
            _httpClient = httpClient;
            _db = db;
        }

        public async Task ImporterProduitsAsync()
        {
            var response = await _httpClient.GetAsync("https://dummyjson.com/products?limit=20");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonObj = JObject.Parse(jsonString);
            var produitsJson = jsonObj["products"] as JArray;

            if (produitsJson is null)
            {
                return;
            }

            foreach (var item in produitsJson)
            {
                var produit = new Produit
                {
                    Nom = item["title"]?.ToString() ?? string.Empty,
                    Description = item["description"]?.ToString() ?? string.Empty,
                    Prix = item["price"]?.ToObject<decimal>() ?? 0m,
                    UrlImage = item["thumbnail"]?.ToString() ?? string.Empty,
                    CategorieNom = item["category"]?.ToString() ?? string.Empty
                };

                // Vérifie si le produit existe déjà (évite les doublons)
                if (!_db.Produits.Any(p => p.Nom == produit.Nom))
                {
                    _db.Produits.Add(produit);
                }
            }

            await _db.SaveChangesAsync();
        }
    }
}
