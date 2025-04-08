
using System.ComponentModel.DataAnnotations;

namespace PaymentTransaction.Models.DTO
{

  public class UpdateProviderRequestDto {

    [Required]
    [MaxLength(50, ErrorMessage = "Provider Name has to be a maximum of 50 characters")]
    public required string ProviderName { get; set; }
  }
}