using MaBoutique.Models;
using MaBoutique.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaBoutique.Controllers
{
    public class CompteController : Controller
    {
        private readonly IUserApiClient _userApiClient;

        public CompteController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        [HttpGet]
        public IActionResult Inscription()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Inscription(Utilisateur utilisateur)
        {
            if (!ModelState.IsValid)
            {
                return View(utilisateur);
            }

            try
            {
                var createdUser = await _userApiClient.RegisterAsync(utilisateur);
                if (createdUser != null)
                {
                    return RedirectToAction("Connexion");
                }
            }
            catch
            {
            }

            ModelState.AddModelError(string.Empty, "Impossible de créer le compte. Le service utilisateur est indisponible ou l'email existe déjà.");
            return View(utilisateur);
        }

        [HttpGet]
        public IActionResult Connexion()
        {
            return View(new Utilisateur());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Connexion(string email, string motDePasse)
        {
            var utilisateur = await _userApiClient.LoginAsync(email, motDePasse);

            if (utilisateur != null)
            {
                HttpContext.Session.SetInt32("UtilisateurId", utilisateur.Id);
                HttpContext.Session.SetString("UtilisateurNom", utilisateur.Nom);
                HttpContext.Session.SetString("UtilisateurRole", utilisateur.Role.ToString());

                return RedirectToAction("Profil");
            }

            ViewBag.MessageErreur = "Identifiants invalides ou service utilisateur indisponible.";
            return View(new Utilisateur { Email = email });
        }

        [HttpGet]
        public async Task<IActionResult> Profil()
        {
            var utilisateurId = HttpContext.Session.GetInt32("UtilisateurId");
            if (utilisateurId == null)
            {
                return RedirectToAction("Connexion");
            }

            var utilisateur = await _userApiClient.GetByIdAsync(utilisateurId.Value);
            if (utilisateur == null)
            {
                return RedirectToAction("Connexion");
            }

            return View(utilisateur);
        }

        public IActionResult Deconnexion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
