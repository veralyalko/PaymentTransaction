using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentTransaction.Models.DTO
{

  public class AddTransactionRequestDto {
    public required double Amount { get; set; }

    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    // public required DateTime Timestamp { get; set; }
    public required string PayerEmail { get; set; }
    public required Guid CurrencyId { get; set; }
    public required Guid ProviderId { get; set; }
    public required Guid StatusId { get; set; }
    public required Guid PaymentMethodId { get; set; }

  }
}