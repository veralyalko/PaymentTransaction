using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.Domain
{

  public class Status {

    public Guid StatusId { get; set; }
    public required string StatusName { get; set; }

    // Constructor to initialize StatusId with a new GUID
    public Status()
    {
      StatusId = Guid.NewGuid();  // Automatically generate a new GUID
    }
    
  }
}