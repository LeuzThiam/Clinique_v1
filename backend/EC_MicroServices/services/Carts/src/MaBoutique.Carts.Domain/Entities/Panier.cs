using System.ComponentModel.DataAnnotations.Schema;

namespace MaBoutique.Carts.Domain.Entities;

public class Panier
{
    public int Id { get; set; }
    public int IdUtilisateur { get; set; }

    public ICollection<ArticlePanier> ArticlesPaniers { get; set; } = new List<ArticlePanier>();

    [NotMapped]
    public decimal Total => ArticlesPaniers?.Sum(a => a.PrixUnitaire * a.Quantite) ?? 0;
}
