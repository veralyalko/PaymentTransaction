using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.Domain
{

  public class PaymentMethod {

    public Guid PaymentMethodId { get; set; }
    public required string PaymentMethodName { get; set; }
    
    // Constructor to initialize PaymentMethodId with a new GUID
    public PaymentMethod()
    {
      PaymentMethodId = Guid.NewGuid();  // Automatically generate a new GUID
    }
  }
}