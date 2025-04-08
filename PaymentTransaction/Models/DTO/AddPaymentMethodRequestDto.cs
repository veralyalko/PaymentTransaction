using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.DTO
{

  public class AddPaymentMethodRequestDto {
    public required string PaymentMethodName { get; set; }
  }
}