using EC_Order_Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace EC_Order_Service.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private static readonly List<Commande> _orders =
        [
            new Commande
            {
                Id = 1,
                UtilisateurId = 1,
                EstPayee = true,
                Total = 129.99m,
                DateCommande = DateTime.UtcNow.AddDays(-3),
                ArticlesCommandes =
                [
                    new ArticleCommande
                    {
                        ProduitId = 1,
                        NomProduit = "Casque Bluetooth",
                        Quantite = 1,
                        PrixUnitaire = 129.99m
                    }
                ]
            }
        ];

        [HttpGet]
        public ActionResult<IEnumerable<Commande>> GetOrders([FromQuery] int? userId)
        {
            IEnumerable<Commande> orders = _orders;

            if (userId.HasValue)
            {
                orders = orders.Where(o => o.UtilisateurId == userId.Value);
            }

            return Ok(orders.OrderByDescending(o => o.DateCommande));
        }

        [HttpGet("{id:int}")]
        public ActionResult<Commande> GetOrderById(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            return order is null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public ActionResult<Commande> CreateOrder([FromBody] Commande order)
        {
            order.Id = _orders.Count == 0 ? 1 : _orders.Max(o => o.Id) + 1;
            order.DateCommande = order.DateCommande == default ? DateTime.UtcNow : order.DateCommande;
            order.Total = order.ArticlesCommandes.Sum(a => a.PrixUnitaire * a.Quantite);

            _orders.Add(order);

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }
    }
}
