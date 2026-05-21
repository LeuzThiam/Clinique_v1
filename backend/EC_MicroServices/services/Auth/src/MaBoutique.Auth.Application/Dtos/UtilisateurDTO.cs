namespace MaBoutique.Auth.Application.Dtos
{
    public class UtilisateurDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? MotDePasse { get; set; }
    }
}
