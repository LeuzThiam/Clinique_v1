namespace EC_Payment_Service.Models;

public class PaymentIntentResult
{
    public string ClientSecret { get; set; } = string.Empty;
    public long Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
}
