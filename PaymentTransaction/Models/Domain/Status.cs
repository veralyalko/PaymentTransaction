using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.Domain
{

  public class Status {

    public required Guid StatusId { get; set; }
    public required string StatusName { get; set; }
    
  }
}