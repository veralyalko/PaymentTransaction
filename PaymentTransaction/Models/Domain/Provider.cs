using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.Domain
{

  public class Provider {

    public Guid ProviderId { get; set; }
    public string ProviderName { get; set; }
    
  }
}