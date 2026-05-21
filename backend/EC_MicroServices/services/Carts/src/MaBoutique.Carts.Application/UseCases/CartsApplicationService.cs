using MaBoutique.Carts.Application.Abstractions;
using MaBoutique.Carts.Application.Dtos;
using MaBoutique.Carts.Domain.Entities;

namespace MaBoutique.Carts.Application.UseCases;

public class CartsApplicationService : ICartsApplicationService
{
    private readonly ICartRepository _repository;

    public CartsApplicationService(ICartRepository repository)
    {
        _repository = repository;
    }

    public async Task<PanierDTO?> GetPanierAsync(int utilisateurId, CancellationToken cancellationToken = default)
    {
        var panier = await _repository.GetByUserIdAsync(utilisateurId, cancellationToken);
        if (panier == null)
            return null;

        return new PanierDTO
        {
            IdUtilisateur = utilisateurId,
            Total = panier.Total,
            Articles = panier.ArticlesPaniers.Select(a => new ArticleAjoutDTO
            {
                ProduitId = a.ProduitId,
                PrixUnitaire = a.PrixUnitaire,
                Quantite = a.Quantite
            }).ToList()
        };
    }

    public async Task AjouterArticleAsync(int utilisateurId, ArticleAjoutDTO dto, CancellationToken cancellationToken = default)
    {
        var panier = await _repository.GetByUserIdAsync(utilisateurId, cancellationToken);

        if (panier == null)
        {
            panier = new Panier { IdUtilisateur = utilisateurId };
            await _repository.AddAsync(panier, cancellationToken);
        }

        var article = panier.ArticlesPaniers.FirstOrDefault(a => a.ProduitId == dto.ProduitId);
        if (article != null)
        {
            article.Quantite += dto.Quantite;
        }
        else
        {
            panier.ArticlesPaniers.Add(new ArticlePanier
            {
                ProduitId = dto.ProduitId,
                PrixUnitaire = dto.PrixUnitaire,
                Quantite = dto.Quantite
            });
        }

        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<SupprimerArticlePanierResult> SupprimerArticleAsync(int utilisateurId, int produitId, CancellationToken cancellationToken = default)
    {
        var panier = await _repository.GetByUserIdAsync(utilisateurId, cancellationToken);
        if (panier == null)
            return SupprimerArticlePanierResult.PanierIntrouvable;

        var article = panier.ArticlesPaniers.FirstOrDefault(a => a.ProduitId == produitId);
        if (article == null)
            return SupprimerArticlePanierResult.ArticleIntrouvable;

        _repository.RemoveArticle(article);
        await _repository.SaveChangesAsync(cancellationToken);

        return SupprimerArticlePanierResult.Success;
    }
}
