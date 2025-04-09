
using System.ComponentModel.DataAnnotations;

namespace PaymentTransaction.Models.DTO
{
    public class AddStatusRequestDto {

        [Required]
        [MaxLength(10, ErrorMessage = "Status Name has to be a maximum of 10 characters")]
        public required string StatusName { get; set; }
    }
}