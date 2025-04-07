using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.DTO
{

  public class StatusDto {

    public required Guid StatusId { get; set; }
    public required string StatusName { get; set; }
    
  }
}