using System.Globalization;
using Microsoft.Net.Http.Headers;
using PaymentTransaction.Models.Domain;
using Swashbuckle.AspNetCore.Annotations;
using PaymentTransaction.Attributes;

namespace PaymentTransaction.Models.DTO
{

  public class ProviderDto {

    [SwaggerSchema(Description = "Unique identifier for the provider")]
    public required Guid ProviderId { get; set; }
    [SwaggerSchemaExample("PayPal")]
    public required string ProviderName { get; set; }
    [SwaggerSchemaExample("PayPal")]
    public ProviderType Type { get; set; }
    [SwaggerSchemaExample("{\"Config1\": \"Value1\", \"Config2\": \"Value2\"}")]
    public string? MetadataJson { get; set; }
    
  }
}