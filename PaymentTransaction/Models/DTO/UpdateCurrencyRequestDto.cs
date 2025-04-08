
using System.ComponentModel.DataAnnotations;

namespace PaymentTransaction.Models.DTO
{

  public class UpdateCurrencyRequestDto {
    
    [Required]
    [MinLength(3, ErrorMessage = "Currency Name has to be a minimum of 3 characters")]
    [MaxLength(3, ErrorMessage = "Currency Name has to be a maximum of 3 characters")]
    public required string CurrencyName { get; set; }
  }
}