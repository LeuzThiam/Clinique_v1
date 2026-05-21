using MaBoutique.Models;
using MaBoutique.Services;
using MaBoutique.Services.ApiModels;
using Microsoft.AspNetCore.Mvc;

namespace MaBoutique.Controllers
{
    public class CommandesController : Controller
    {
        private readonly IOrderApiClient _orderApiClient;

        public CommandesController(IOrderApiClient orderApiClient)
        {
            _orderApiClient = orderApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var clientId = HttpContext.Session.GetInt32("UtilisateurId") ?? 1;

            try
            {
                var remoteOrders = await _orderApiClient.GetOrdersAsync(clientId);
                ViewBag.DataSource = "api";
                return View(remoteOrders.Select(MapToDomainModel).ToList());
            }
            catch
            {
                ViewBag.ErrorMessage = "Le service de commandes est indisponible.";
                return View(new List<Commande>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var remoteOrder = await _orderApiClient.GetOrderAsync(id);
                if (remoteOrder == null)
                {
                    return NotFound();
                }

                ViewBag.DataSource = "api";
                return View(MapToDomainModel(remoteOrder));
            }
            catch
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
        }

        private static Commande MapToDomainModel(OrderApiModel order)
        {
            return new Commande
            {
                Id = order.Id,
                DateCommande = order.DateCommande,
                EstPayee = order.EstPayee,
                Total = order.Total,
                UtilisateurId = order.UtilisateurId,
                ArticlesCommandes = order.ArticlesCommandes.Select(item => new ArticleCommande
                {
                    ProduitId = item.ProduitId,
                    Quantite = item.Quantite,
                    PrixUnitaire = item.PrixUnitaire,
                    Produit = new Produit
                    {
                        Id = item.ProduitId,
                        Nom = item.NomProduit,
                        Prix = item.PrixUnitaire
                    }
                }).ToList()
            };
        }
    }
}
