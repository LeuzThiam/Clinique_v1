namespace MaBoutique.Carts.Domain.Entities;

public class Commande
{
    public int Id { get; set; }
    public int UtilisateurId { get; set; }
    public DateTime DateCommande { get; set; }
    public bool EstPayee { get; set; }
    public decimal Total { get; set; }
}
