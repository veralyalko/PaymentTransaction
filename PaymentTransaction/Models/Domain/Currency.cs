using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace PaymentTransaction.Models.Domain
{
    public class Currency 
    {

        public Guid CurrencyId { get; set; }
        public required string CurrencyName { get; set; }
        // Constructor to initialize CurrencyId with a new GUID
        public Currency()
        {
            CurrencyId = Guid.NewGuid();  // Automatically generate a new GUID
        }
    }
}