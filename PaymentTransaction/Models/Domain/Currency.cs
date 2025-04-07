using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.Domain
{

  public class Currency {

    public required Guid CurrencyId { get; set; }
    public required string CurrencyName { get; set; }
    
  }
}