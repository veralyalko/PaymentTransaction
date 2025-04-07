using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.Domain
{

  public class Currency {

    public Guid CurrencyId { get; set; }
    public string CurrencyName { get; set; }
    
  }
}