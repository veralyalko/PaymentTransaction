using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.Domain
{

  public class Provider
    {
        public Guid ProviderId { get; set; }
        public required string ProviderName { get; set; }

        // Constructor to initialize ProviderId with a new GUID
        public Provider()
        {
            ProviderId = Guid.NewGuid();  // Automatically generate a new GUID
        }
    }

}