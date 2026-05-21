namespace EC_Cart_Service.Models;

public class UpsertCartItemRequest
{
    public int ProduitId { get; set; }

    public int Quantite { get; set; } = 1;

    public string NomProduit { get; set; } = string.Empty;

    public decimal PrixUnitaire { get; set; }

    public string UrlImage { get; set; } = string.Empty;
}
