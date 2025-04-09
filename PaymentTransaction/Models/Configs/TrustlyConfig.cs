namespace PaymentTransaction.Models.Configs
{
    public class TrustlyConfig
    {        
        public string? Region { get; set; } // example of use: "EU" or "US"
        public string? Currency { get; set; } // example of use: "EUR" or "UAN"
    }
}