using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PaymentTransaction.Data
{
    public class PaymentTransactionAuthDbContext: IdentityDbContext
    {
        public PaymentTransactionAuthDbContext(DbContextOptions<PaymentTransactionAuthDbContext> options): base(options)
        {

        }
    }
}