
using Swashbuckle.AspNetCore.Annotations;
using PaymentTransaction.Attributes;

namespace PaymentTransaction.Models.Domain
{
    public class Provider
    {
        [SwaggerSchema(Description = "Unique identifier for the provider")]
        public Guid ProviderId { get; set; }
        
        [SwaggerSchemaExample("PayPal")]
        public required string ProviderName { get; set; }

        public Provider()
        {
            ProviderId = Guid.NewGuid(); 
        }

        [SwaggerSchemaExample("PayPal")]
        public ProviderType Type { get; set; }

        // Provider Other Configurations
        [SwaggerSchemaExample("{\"Config1\": \"Value1\", \"Config2\": \"Value2\"}")]
        public string? MetadataJson { get; set; }
    }

}