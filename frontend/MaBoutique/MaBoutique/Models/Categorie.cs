using System.Collections.Generic;

namespace MaBoutique.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;

        public ICollection<Produit> Produits { get; set; } = new List<Produit>();
    }
}
