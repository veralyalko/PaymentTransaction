using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.DTO
{

  public class UpdateStatusRequestDto {
    public required string StatusName { get; set; }
  }
}