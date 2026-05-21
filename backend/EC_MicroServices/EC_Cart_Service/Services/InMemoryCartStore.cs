using EC_Cart_Service.Models;
using System.Collections.Concurrent;

namespace EC_Cart_Service.Services;

public class InMemoryCartStore : ICartStore
{
    private readonly ConcurrentDictionary<int, Cart> _carts = new();

    public Cart GetOrCreate(int utilisateurId)
    {
        return _carts.GetOrAdd(utilisateurId, id => new Cart { UtilisateurId = id });
    }

    public Cart AddItem(int utilisateurId, UpsertCartItemRequest request)
    {
        var cart = GetOrCreate(utilisateurId);
        var article = cart.Articles.FirstOrDefault(item => item.ProduitId == request.ProduitId);

        if (article is null)
        {
            cart.Articles.Add(new CartItem
            {
                ProduitId = request.ProduitId,
                Quantite = request.Quantite,
                NomProduit = request.NomProduit,
                PrixUnitaire = request.PrixUnitaire,
                UrlImage = request.UrlImage
            });
        }
        else
        {
            article.Quantite += request.Quantite;

            if (!string.IsNullOrWhiteSpace(request.NomProduit))
            {
                article.NomProduit = request.NomProduit;
            }

            if (request.PrixUnitaire > 0)
            {
                article.PrixUnitaire = request.PrixUnitaire;
            }

            if (!string.IsNullOrWhiteSpace(request.UrlImage))
            {
                article.UrlImage = request.UrlImage;
            }
        }

        cart.DerniereMiseAJourUtc = DateTime.UtcNow;
        return cart;
    }

    public Cart? UpdateItemQuantity(int utilisateurId, int produitId, int quantite)
    {
        var cart = GetOrCreate(utilisateurId);
        var article = cart.Articles.FirstOrDefault(item => item.ProduitId == produitId);

        if (article is null)
        {
            return null;
        }

        article.Quantite = quantite;
        cart.DerniereMiseAJourUtc = DateTime.UtcNow;
        return cart;
    }

    public bool RemoveItem(int utilisateurId, int produitId)
    {
        if (!_carts.TryGetValue(utilisateurId, out var cart))
        {
            return false;
        }

        var article = cart.Articles.FirstOrDefault(item => item.ProduitId == produitId);
        if (article is null)
        {
            return false;
        }

        cart.Articles.Remove(article);
        cart.DerniereMiseAJourUtc = DateTime.UtcNow;
        return true;
    }

    public bool Clear(int utilisateurId)
    {
        if (!_carts.TryGetValue(utilisateurId, out var cart))
        {
            return false;
        }

        cart.Articles.Clear();
        cart.DerniereMiseAJourUtc = DateTime.UtcNow;
        return true;
    }
}
