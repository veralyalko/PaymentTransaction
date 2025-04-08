using System.Globalization;
using Microsoft.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentTransaction.Models.DTO
{

  public class TransactionDto {

    public required Guid Id { get; set; }
     public required double Amount { get; set; }

    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public required DateTime Timestamp { get; set; }
    public required string PayerEmail { get; set; }
    // public required Guid CurrencyId { get; set; }
    // public required Guid ProviderId { get; set; }
    // public required Guid StatusId { get; set; }
    // public required Guid PaymentMethodId { get; set; }

    // Navigation Property
    public required ProviderDto Provider { get; set; }
    public required CurrencyDto Currency { get; set; }
    public required PaymentMethodDto PaymentMethod { get; set; }
    public required StatusDto Status { get; set; }
    
  }
}