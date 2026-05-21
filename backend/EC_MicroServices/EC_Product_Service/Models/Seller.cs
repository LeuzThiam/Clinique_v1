namespace EC_Product_Service.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Actif { get; set; }
    }
}
