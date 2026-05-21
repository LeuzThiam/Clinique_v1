namespace EC_Order_Service.Models
{
    public class Commande
    {
        public int Id { get; set; }
        public DateTime DateCommande { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public bool EstPayee { get; set; }
        public int UtilisateurId { get; set; }
        public List<ArticleCommande> ArticlesCommandes { get; set; } = [];
    }
}
