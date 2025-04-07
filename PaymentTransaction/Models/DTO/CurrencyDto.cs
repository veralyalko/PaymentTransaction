using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.DTO
{

  public class CurrencyDto {

    public required Guid CurrencyId { get; set; }
    public required string CurrencyName { get; set; }
    
  }
}