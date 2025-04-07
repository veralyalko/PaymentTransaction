using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.Domain
{

  public class PaymentMethod {

    public required Guid PaymentMethodId { get; set; }
    public required string PaymentMethodName { get; set; }
    
  }
}