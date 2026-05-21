using EC_Cart_Service.Models;

namespace EC_Cart_Service.Services;

public interface ICartStore
{
    Cart GetOrCreate(int utilisateurId);

    Cart AddItem(int utilisateurId, UpsertCartItemRequest request);

    Cart? UpdateItemQuantity(int utilisateurId, int produitId, int quantite);

    bool RemoveItem(int utilisateurId, int produitId);

    bool Clear(int utilisateurId);
}
