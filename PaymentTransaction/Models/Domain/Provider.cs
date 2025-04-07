using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.Domain
{

  public class Provider {

    public required Guid ProviderId { get; set; }
    public required string ProviderName { get; set; }
    
  }
}