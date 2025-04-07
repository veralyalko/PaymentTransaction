using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.Domain
{

  public class PaymentMethod {

    public Guid PaymentMethodId { get; set; }
    public string PaymentMethodName { get; set; }
    
  }
}