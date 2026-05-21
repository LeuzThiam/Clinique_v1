using EC_Product_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EC_Product_Service.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private static readonly List<Product> _fallbackProducts =
        [
            new Product
            {
                Id = 1,
                Nom = "Casque Bluetooth",
                Description = "Casque sans fil avec reduction de bruit.",
                Prix = 129.99m,
                Quantite = 15,
                DateAjout = DateTime.UtcNow,
                UrlImage = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?auto=format&fit=crop&w=900&q=80",
                CategorieNom = "audio",
                VendeurId = 1
            },
            new Product
            {
                Id = 2,
                Nom = "Clavier mecanique",
                Description = "Clavier compact retroeclaire pour bureautique et jeu.",
                Prix = 89.50m,
                Quantite = 20,
                DateAjout = DateTime.UtcNow.AddMinutes(-5),
                UrlImage = "https://images.unsplash.com/photo-1511467687858-23d96c32e4ae?auto=format&fit=crop&w=900&q=80",
                CategorieNom = "informatique",
                VendeurId = 1
            },
            new Product
            {
                Id = 3,
                Nom = "Montre connectee",
                Description = "Suivi d'activite, notifications et autonomie longue duree.",
                Prix = 159.00m,
                Quantite = 10,
                DateAjout = DateTime.UtcNow.AddMinutes(-10),
                UrlImage = "https://images.unsplash.com/photo-1546868871-7041f2a55e12?auto=format&fit=crop&w=900&q=80",
                CategorieNom = "wearables",
                VendeurId = 2
            }
        ];

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string? search)
        {
            try
            {
                IQueryable<Product> query = _context.Produits.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(p =>
                        p.Nom.Contains(search) ||
                        p.CategorieNom.Contains(search));
                }

                var products = await query
                    .OrderByDescending(p => p.DateAjout)
                    .ToListAsync();

                if (products.Count > 0)
                {
                    return Ok(products);
                }
            }
            catch
            {
            }

            IEnumerable<Product> fallback = _fallbackProducts;
            if (!string.IsNullOrWhiteSpace(search))
            {
                fallback = fallback.Where(p =>
                    p.Nom.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.CategorieNom.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            return Ok(fallback.OrderByDescending(p => p.DateAjout));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try
            {
                var product = await _context.Produits.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if (product is not null)
                {
                    return Ok(product);
                }
            }
            catch
            {
            }

            var fallbackProduct = _fallbackProducts.FirstOrDefault(p => p.Id == id);
            return fallbackProduct is null ? NotFound() : Ok(fallbackProduct);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct([FromBody] Product model)
        {
            model.DateAjout = model.DateAjout == default ? DateTime.UtcNow : model.DateAjout;

            try
            {
                _context.Produits.Add(model);
                await _context.SaveChangesAsync();
            }
            catch
            {
                model.Id = _fallbackProducts.Count == 0 ? 1 : _fallbackProducts.Max(p => p.Id) + 1;
                _fallbackProducts.Add(model);
            }

            return CreatedAtAction(nameof(GetProductById), new { id = model.Id }, model);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product model)
        {
            try
            {
                var existingProduct = await _context.Produits.FirstOrDefaultAsync(p => p.Id == id);
                if (existingProduct is null)
                {
                    return NotFound();
                }

                existingProduct.Nom = model.Nom;
                existingProduct.Description = model.Description;
                existingProduct.Prix = model.Prix;
                existingProduct.Quantite = model.Quantite;
                existingProduct.UrlImage = model.UrlImage;
                existingProduct.CategorieNom = model.CategorieNom;
                existingProduct.VendeurId = model.VendeurId;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                var fallbackProduct = _fallbackProducts.FirstOrDefault(p => p.Id == id);
                if (fallbackProduct is null)
                {
                    return NotFound();
                }

                fallbackProduct.Nom = model.Nom;
                fallbackProduct.Description = model.Description;
                fallbackProduct.Prix = model.Prix;
                fallbackProduct.Quantite = model.Quantite;
                fallbackProduct.UrlImage = model.UrlImage;
                fallbackProduct.CategorieNom = model.CategorieNom;
                fallbackProduct.VendeurId = model.VendeurId;
                return NoContent();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Produits.FirstOrDefaultAsync(p => p.Id == id);
                if (product is null)
                {
                    return NotFound();
                }

                _context.Produits.Remove(product);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                var fallbackProduct = _fallbackProducts.FirstOrDefault(p => p.Id == id);
                if (fallbackProduct is null)
                {
                    return NotFound();
                }

                _fallbackProducts.Remove(fallbackProduct);
                return NoContent();
            }
        }
    }
}
