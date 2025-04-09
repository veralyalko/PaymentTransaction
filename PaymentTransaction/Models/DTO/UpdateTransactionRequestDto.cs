using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentTransaction.Models.DTO
{
  public class UpdateTransactionRequestDto {
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a valid number, greater than 0.")]
    public required double Amount { get; set; }
    [Required]
    [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
    public required string PayerEmail { get; set; }
    [Required]
    public required Guid CurrencyId { get; set; }
    [Required]
    public required Guid ProviderId { get; set; }
    [Required]
    public required Guid StatusId { get; set; }
    [Required]
    public required Guid PaymentMethodId { get; set; }
  }
}