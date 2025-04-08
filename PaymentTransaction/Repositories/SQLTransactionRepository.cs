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

    public async Task<List<Transaction>> GetAllAsync()
    {
      return await dbContext.Transaction.ToListAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
      return await dbContext.Transaction.SingleOrDefaultAsync(x => x.Id == id);
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

      // TODO
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