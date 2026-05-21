using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MaBoutique.Models
{
    public enum RoleUtilisateur
    {
        [Display(Name = "Client")]
        Client = 0,

        [Display(Name = "Vendeur")]
        Vendeur = 1
    }

    public class Utilisateur
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le prenom est obligatoire.")]
        [StringLength(50)]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(50)]
        public string Nom { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire.")]
        [EmailAddress(ErrorMessage = "Veuillez saisir une adresse email valide.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caracteres.")]
        public string MotDePasse { get; set; }

        [Required(ErrorMessage = "Veuillez selectionner un role.")]
        public RoleUtilisateur Role { get; set; } = RoleUtilisateur.Client;

        [Required(ErrorMessage = "L'adresse est obligatoire.")]
        [StringLength(100)]
        public string Adresse { get; set; }

        [Required(ErrorMessage = "La ville est obligatoire.")]
        [StringLength(50)]
        public string Ville { get; set; }

        [Required(ErrorMessage = "Le code postal est obligatoire.")]
        [StringLength(10)]
        [Display(Name = "Code postal")]
        public string CodePostal { get; set; }

        [Required(ErrorMessage = "La province est obligatoire.")]
        [StringLength(50)]
        public string Province { get; set; }

        [Required(ErrorMessage = "Le pays est obligatoire.")]
        [StringLength(50)]
        public string Pays { get; set; }

        [Required(ErrorMessage = "Le numero de telephone est obligatoire.")]
        [Phone(ErrorMessage = "Veuillez entrer un numero de telephone valide.")]
        [Display(Name = "Telephone")]
        public string Telephone { get; set; }

        [ValidateNever]
        public ICollection<Commande> Commandes { get; set; }

        [ValidateNever]
        public ICollection<Facture> Factures { get; set; }

        [ValidateNever]
        public ICollection<Panier> Paniers { get; set; }

        [ValidateNever]
        public ICollection<Produit> Produits { get; set; }
    }
}
