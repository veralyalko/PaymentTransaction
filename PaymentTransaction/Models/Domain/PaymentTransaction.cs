using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.Domain
{

  public class PaymentTransaction {

    public Guid Id { get; set; }
    public Guid ProviderId { get; set; }
    public double Amount { get; set; }
    public Guid CurrencyId { get; set; }
    public string PaymentMethodId { get; set; }
    public string StatusId { get; set; }
    public DateTime Timestamp { get; set; }
    public string PayerEmail { get; set; }



    // Property
    public Provider Provider { get; set; }
    public Currency Currency { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public Status Status { get; set; }
    
  }
}