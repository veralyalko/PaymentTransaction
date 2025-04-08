
using System.ComponentModel.DataAnnotations;

namespace PaymentTransaction.Models.DTO
{

  public class AddStatusRequestDto {

    [Required]
    [MaxLength(30, ErrorMessage = "Status Name has to be a maximum of 30 characters")]
    public required string StatusName { get; set; }
  }
}