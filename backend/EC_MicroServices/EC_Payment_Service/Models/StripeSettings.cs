namespace EC_Payment_Service.Models;

public class StripeSettings
{
    public const string SectionName = "Stripe";

    public string SecretKey { get; set; } = string.Empty;
    public string PublishableKey { get; set; } = string.Empty;

    public bool IsConfigured()
    {
        return !string.IsNullOrWhiteSpace(SecretKey)
            && !string.IsNullOrWhiteSpace(PublishableKey)
            && !SecretKey.StartsWith("CHANGE_ME_", StringComparison.OrdinalIgnoreCase)
            && !PublishableKey.StartsWith("CHANGE_ME_", StringComparison.OrdinalIgnoreCase);
    }
}
