namespace EC_Cart_Service.Models;

public class Cart
{
    public int UtilisateurId { get; set; }

    public List<CartItem> Articles { get; set; } = [];

    public DateTime DerniereMiseAJourUtc { get; set; } = DateTime.UtcNow;

    public decimal Total => Articles.Sum(article => article.PrixUnitaire * article.Quantite);
}
