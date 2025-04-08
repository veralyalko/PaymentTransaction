using Microsoft.EntityFrameworkCore;
using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Data
{
    public class PaymentTransactionDbContext: DbContext
    {
        public PaymentTransactionDbContext(DbContextOptions<PaymentTransactionDbContext> DbContextOptions): base(DbContextOptions)
        {

        }

        public DbSet<Currency> Currency { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<Provider> Provider { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
       // 👇 Method to configure Timestamp
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Timestamp)
                .IsRequired()
                .ValueGeneratedNever(); 

            base.OnModelCreating(modelBuilder);
        }

    }
}