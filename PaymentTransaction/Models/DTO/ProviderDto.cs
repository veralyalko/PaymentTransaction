using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.DTO
{

  public class ProviderDto {

    public required Guid ProviderId { get; set; }
    public required string ProviderName { get; set; }
    
  }
}