using System.Collections.Generic;

namespace MaBoutique.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public ICollection<Produit> Produits { get; set; }
    }
}
