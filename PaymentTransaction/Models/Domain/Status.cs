using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.Domain
{

  public class Status {

    public Guid StatusId { get; set; }
    public string StatusName { get; set; }
    
  }
}