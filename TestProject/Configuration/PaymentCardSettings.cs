namespace AutomationProject.Configuration;

/// <summary>
/// Payment card data for checkout (test/demo). Stored in appsettings.json, not real credentials.
/// </summary>
public class PaymentCardSettings
{
    /// <summary>Card number (e.g. test value 4111111111111111).</summary>
    public string CardNumber { get; set; } = "4111111111111111";

    /// <summary>CVC/CVV.</summary>
    public string Cvc { get; set; } = "123";

    /// <summary>Expiry month (1–12).</summary>
    public string ExpiryMonth { get; set; } = "12";

    /// <summary>Expiry year (e.g. 2030).</summary>
    public string ExpiryYear { get; set; } = "2030";
}
