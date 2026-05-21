namespace EC_Cart_Service.Models;

public class CartItem
{
    public int ProduitId { get; set; }

    public string NomProduit { get; set; } = string.Empty;

    public decimal PrixUnitaire { get; set; }

    public string UrlImage { get; set; } = string.Empty;

    public int Quantite { get; set; }
}
