using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Models
{
    public class Utilisateur : IdentityUser
    {
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        [StringLength(50, ErrorMessage = "Le prénom ne peut pas dépasser 50 caractères.")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "L'adresse e-mail est obligatoire.")]
        [EmailAddress(ErrorMessage = "Format d'adresse e-mail invalide.")]
        public override string Email { get; set; }

        [Phone(ErrorMessage = "Format de numéro invalide.")]
        public override string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Le rôle est obligatoire.")]
        [StringLength(20, ErrorMessage = "Le rôle ne peut pas dépasser 20 caractères.")]
        public string Role { get; set; } 
    }
}
