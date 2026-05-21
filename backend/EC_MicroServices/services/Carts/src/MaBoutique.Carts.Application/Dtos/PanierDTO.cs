namespace MaBoutique.Carts.Application.Dtos;

public class PanierDTO
{
    public int IdUtilisateur { get; set; }
    public decimal Total { get; set; }
    public List<ArticleAjoutDTO> Articles { get; set; } = new();
}
