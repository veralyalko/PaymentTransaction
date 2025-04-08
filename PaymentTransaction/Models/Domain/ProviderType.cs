using System.Text.Json.Serialization;

namespace PaymentTransaction.Models.Domain
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProviderType
    {
        PayPal = 1,
        Trustly = 2,
        OtherProvider = 3
    }
}