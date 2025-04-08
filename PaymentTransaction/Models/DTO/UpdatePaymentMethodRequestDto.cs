
using System.ComponentModel.DataAnnotations;

namespace PaymentTransaction.Models.DTO
{

  public class UpdatePaymentMethodRequestDto {
    
    [Required]
    [MaxLength(30, ErrorMessage = "Payment Method Name has to be a maximum of 30 characters")]
    public required string PaymentMethodName { get; set; }
  }
}