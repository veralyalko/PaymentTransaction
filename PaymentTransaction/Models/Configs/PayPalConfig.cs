namespace PaymentTransaction.Models.Configs
{
    public class PayPalConfig
    {
        public string? Mode { get; set; } // example of use: "test" or "live"
        public string? defaultCurrency { get; set; } // example of use: "USD"
    }
}