namespace EC_Order_Service.Models
{
    public class ArticleCommande
    {
        public int ProduitId { get; set; }
        public string NomProduit { get; set; } = string.Empty;
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
    }
}
