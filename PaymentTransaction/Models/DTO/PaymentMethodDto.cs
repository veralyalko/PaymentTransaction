
namespace PaymentTransaction.Models.DTO
{
    public class PaymentMethodDto {
        public required Guid PaymentMethodId { get; set; }
        public required string PaymentMethodName { get; set; }
        
    }
}