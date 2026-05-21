using MaBoutique.Carts.Application.Dtos;
using MaBoutique.Carts.Application.UseCases;

namespace MaBoutique.Carts.Application.Abstractions;

public interface ICartsApplicationService
{
    Task<PanierDTO?> GetPanierAsync(int utilisateurId, CancellationToken cancellationToken = default);
    Task AjouterArticleAsync(int utilisateurId, ArticleAjoutDTO dto, CancellationToken cancellationToken = default);
    Task<SupprimerArticlePanierResult> SupprimerArticleAsync(int utilisateurId, int produitId, CancellationToken cancellationToken = default);
}
