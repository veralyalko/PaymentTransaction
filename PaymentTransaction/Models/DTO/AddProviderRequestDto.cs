using System.ComponentModel.DataAnnotations;
using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Models.DTO
{
    public class AddProviderRequestDto {

        [Required]
        [MaxLength(50, ErrorMessage = "Provider Name has to be a maximum of 50 characters")]
        public required string ProviderName { get; set; }

        [Required]
        public ProviderType Type { get; set; }
        public string? MetadataJson { get; set; }
    }
}