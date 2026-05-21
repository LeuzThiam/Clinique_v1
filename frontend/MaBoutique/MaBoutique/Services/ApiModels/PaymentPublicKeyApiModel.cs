namespace MaBoutique.Services.ApiModels;

public class PaymentPublicKeyApiModel
{
    public string Key { get; set; } = string.Empty;
    public bool Configured { get; set; }
}
