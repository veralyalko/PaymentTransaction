using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.DTO
{

  public class AddCurrencyRequestDto {
    public required string CurrencyName { get; set; }
  }
}