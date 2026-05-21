using Microsoft.AspNetCore.Mvc;
using MaBoutique.Models;
using System.Linq;

namespace MaBoutique.Controllers
{
    public class FacturesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacturesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Factures/
        public IActionResult Index()
        {
            // Exemple : récupérer toutes les factures (tu pourras filtrer par utilisateur connecté plus tard)
            var factures = _context.Factures
                .Select(f => new Facture
                {
                    Id = f.Id,
                    DateFacturation = f.DateFacturation,
                    NumeroFacture = f.NumeroFacture,
                    MontantTotal = f.MontantTotal,
                    Utilisateur = f.Utilisateur
                })
                .ToList();

            return View(factures);
        }

        // GET: /Factures/Details/5
        public IActionResult Details(int id)
        {
            var facture = _context.Factures
                .Where(f => f.Id == id)
                .FirstOrDefault();

            if (facture == null)
                return NotFound();

            return View(facture);
        }

        // GET: /Factures/TelechargerPdf/5
        public IActionResult TelechargerPdf(int id)
        {
            // Ici, tu pourrais générer le PDF plus tard
            return RedirectToAction("Details", new { id });
        }
    }
}
