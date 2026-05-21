using MaBoutique.Carts.Application.Abstractions;
using MaBoutique.Carts.Application.Dtos;
using MaBoutique.Carts.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaBoutique.Carts.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaniersController : ControllerBase
    {
        private readonly ICartsApplicationService _cartsService;

        public PaniersController(ICartsApplicationService cartsService)
        {
            _cartsService = cartsService;
        }

        [HttpGet("{utilisateurId}")]
        public async Task<IActionResult> GetPanier(int utilisateurId, CancellationToken cancellationToken)
        {
            var panier = await _cartsService.GetPanierAsync(utilisateurId, cancellationToken);

            if (panier == null)
                return NotFound(new { message = "Panier introuvable." });

            return Ok(panier);
        }

        [HttpPost("{utilisateurId}/ajouter")]
        public async Task<IActionResult> AjouterArticle(int utilisateurId, [FromBody] ArticleAjoutDTO dto, CancellationToken cancellationToken)
        {
            await _cartsService.AjouterArticleAsync(utilisateurId, dto, cancellationToken);
            return Ok(new { message = "Article ajouté au panier." });
        }

        [HttpDelete("{utilisateurId}/supprimer/{produitId}")]
        public async Task<IActionResult> SupprimerArticle(int utilisateurId, int produitId, CancellationToken cancellationToken)
        {
            var result = await _cartsService.SupprimerArticleAsync(utilisateurId, produitId, cancellationToken);

            return result switch
            {
                SupprimerArticlePanierResult.PanierIntrouvable => NotFound(new { message = "Panier introuvable." }),
                SupprimerArticlePanierResult.ArticleIntrouvable => NotFound(new { message = "Article introuvable dans le panier." }),
                _ => NoContent()
            };
        }
    }
}
