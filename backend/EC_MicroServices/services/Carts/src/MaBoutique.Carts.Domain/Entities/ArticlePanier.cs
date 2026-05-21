namespace MaBoutique.Carts.Domain.Entities;

public class ArticlePanier
{
    public int Id { get; set; }
    public int ProduitId { get; set; }
    public decimal PrixUnitaire { get; set; }
    public int Quantite { get; set; }

    public int PanierId { get; set; }
    public Panier Panier { get; set; } = null!;
}
