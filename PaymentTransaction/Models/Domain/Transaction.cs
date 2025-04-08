using System.Globalization;
using Microsoft.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentTransaction.Models.Domain
{

  public class Transaction {

    public Guid Id { get; set; }
    public required Guid ProviderId { get; set; }
    public required double Amount { get; set; }
    public required Guid CurrencyId { get; set; }
    public required string PaymentMethodId { get; set; }
    public required string StatusId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public required DateTime Timestamp { get; set; }
    public required string PayerEmail { get; set; }

    // Constructor to initialize Id with a new GUID
    public Transaction()
    {
      Id = Guid.NewGuid();  // Automatically generate a new GUID
    }

    // Property
    public required Provider Provider { get; set; }
    public required Currency Currency { get; set; }
    public required PaymentMethod PaymentMethod { get; set; }
    public required Status Status { get; set; }
    
  }
}