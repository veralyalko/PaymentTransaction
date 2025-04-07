using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.DTO
{

  public class AddProviderRequestDto {
    public required string ProviderName { get; set; }
    
  }
}