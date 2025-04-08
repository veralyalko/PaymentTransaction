using Microsoft.EntityFrameworkCore;
using PaymentTransaction.Data;
using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Repositories
{
  public class SQLTransactionRepository : ITransactionRepository
  {
    private readonly PaymentTransactionDbContext dbContext;
    public SQLTransactionRepository(PaymentTransactionDbContext dbContext)
    {
      this.dbContext = dbContext;
    }
    
    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
      await dbContext.Transaction.AddAsync(transaction);
      await dbContext.SaveChangesAsync();

      return transaction;
    }

    public async Task<List<Transaction>> GetAllAsync(
      string? filterOn = null, 
      string? filterQuery = null,
      DateTime? fromDate = null,
      DateTime? toDate = null)
    {
      var transaction = dbContext.Transaction
        .Include(t => t.Provider)
        .Include(t => t.Currency)
        .Include(t => t.PaymentMethod)
        .Include(t => t.Status)
        .AsQueryable();

        // Apply filter if not null
        if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
        {
            // by ProviderName
            if (filterOn.Equals("providerName", StringComparison.OrdinalIgnoreCase))
            {
                transaction = transaction.Where(t => t.Provider.ProviderName.Contains(filterQuery));
            }

            // by Status
            if (filterOn.Equals("Status", StringComparison.OrdinalIgnoreCase))
            {
                transaction = transaction.Where(t => t.Status.StatusName.Contains(filterQuery));
            }


        }

        // Filter Date range (from/to)
        if (fromDate.HasValue)
        {
            transaction = transaction.Where(t => t.Timestamp >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            var inclusiveTo = toDate.Value.Date.AddDays(1).AddTicks(-1); // include full day
            transaction = transaction.Where(t => t.Timestamp <= inclusiveTo);
        }



      return await transaction.ToListAsync();

    }

    public async Task<Transaction?> CreateForProviderAsync(string providerName, Transaction transaction)
    {
      await dbContext.Transaction.AddAsync(transaction);
      await dbContext.SaveChangesAsync();
      // Reload with related entities

      return await dbContext.Transaction
          .Include(t => t.Provider)
          .Include(t => t.Currency)
          .Include(t => t.PaymentMethod)
          .Include(t => t.Status)
          .SingleOrDefaultAsync(t => t.Id == transaction.Id);

      // return await dbContext.Transaction
      //     .Include(t => t.Provider)
      //     .Include(t => t.Currency)
      //     .Include(t => t.PaymentMethod)
      //     .Include(t => t.Status)
      //     .SingleOrDefaultAsync(t => t.Provider.ProviderName == providerName);
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
      return await dbContext.Transaction.Include("Provider").Include("Currency").Include("PaymentMethod").Include("Status").SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Transaction?> DeleteAsync(Guid id)
    {
      var existingTransaction = await dbContext.Transaction.FirstOrDefaultAsync(x => x.Id == id);

      if (existingTransaction == null)
      {
            return null;
      }

      // Delete the transaction
      dbContext.Transaction.Remove(existingTransaction);
      await dbContext.SaveChangesAsync();
      return existingTransaction;
    }

    public async Task<Transaction?> UpdateAsync(Guid id, Transaction transaction)
    {
      var existingTransaction = await dbContext.Transaction.FirstOrDefaultAsync(x => x.Id == id);

      if (existingTransaction == null)
      {
            return null;
      }

      existingTransaction.Amount = transaction.Amount;
      existingTransaction.PayerEmail = transaction.PayerEmail;
      existingTransaction.Timestamp = transaction.Timestamp;
      existingTransaction.CurrencyId = transaction.CurrencyId;
      existingTransaction.StatusId = transaction.StatusId;
      existingTransaction.PaymentMethodId = transaction.PaymentMethodId;

      await dbContext.SaveChangesAsync();
      return existingTransaction;
    }

    

  }
}