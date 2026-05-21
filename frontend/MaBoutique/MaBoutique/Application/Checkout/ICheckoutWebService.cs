namespace MaBoutique.Application.Checkout;

public interface ICheckoutWebService
{
    Task<(bool Success, int? CommandeId)> FinaliserCommandePayeAsync(int utilisateurId, CancellationToken cancellationToken = default);
}
