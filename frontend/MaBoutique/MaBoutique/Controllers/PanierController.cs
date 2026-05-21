using MaBoutique.Models;
using MaBoutique.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaBoutique.Controllers
{
    public class PanierController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductApiClient _productApiClient;

        public PanierController(ApplicationDbContext context, IProductApiClient productApiClient)
        {
            _context = context;
            _productApiClient = productApiClient;
        }

        public IActionResult Index()
        {
            var panier = ObtenirPanier();

            var total = panier.ArticlesPaniers.Sum(a => (a.Produit?.Prix ?? 0m) * a.Quantite);
            ViewBag.Total = total;

            return View(panier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ajouter(int idProduit, int quantite = 1)
        {
            var produit = _context.Produits.Find(idProduit);
            if (produit == null)
            {
                try
                {
                    var remoteProduct = await _productApiClient.GetProduitAsync(idProduit);
                    if (remoteProduct != null)
                    {
                        produit = new Produit
                        {
                            Id = remoteProduct.Id,
                            Nom = remoteProduct.Nom,
                            Description = remoteProduct.Description,
                            Prix = remoteProduct.Prix,
                            UrlImage = remoteProduct.UrlImage,
                            CategorieNom = remoteProduct.CategorieNom,
                            VendeurId = remoteProduct.VendeurId
                        };

                        _context.Produits.Add(produit);
                        await _context.SaveChangesAsync();
                    }
                }
                catch
                {
                }
            }

            if (produit == null)
            {
                return NotFound();
            }

            var panier = ObtenirPanier();

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

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Supprimer(int idProduit)
        {
            var panier = ObtenirPanier();

            var article = panier.ArticlesPaniers.FirstOrDefault(a => a.ProduitId == idProduit);
            if (article != null)
            {
                _context.ArticlesPaniers.Remove(article);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        private Panier ObtenirPanier()
        {
            var panier = _context.Paniers
                .Include(p => p.ArticlesPaniers)
                .ThenInclude(ap => ap.Produit)
                .FirstOrDefault();

            if (panier == null)
            {
                panier = new Panier
                {
                    ArticlesPaniers = new List<ArticlePanier>()
                };

                _context.Paniers.Add(panier);
                _context.SaveChanges();
            }

            return panier;
        }
    }
}
