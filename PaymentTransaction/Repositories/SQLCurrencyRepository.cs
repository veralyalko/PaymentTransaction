using Microsoft.EntityFrameworkCore;
using PaymentTransaction.Data;
using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Repositories
{
  public class SQLCurrencyRepository : ICurrencyRepository
  {
    private readonly PaymentTransactionDbContext dbContext;
    public SQLCurrencyRepository(PaymentTransactionDbContext dbContext)
    {
      this.dbContext = dbContext;
    }
    public async Task<List<Currency>> GetAllAsync()
    {
      return await dbContext.Currency.ToListAsync();
    }

    public async Task<Currency?> GetByIdAsync(Guid id)
    {
      return await dbContext.Currency.SingleOrDefaultAsync(x => x.CurrencyId == id);
    }

    public async Task<Currency> CreateAsync(Currency currency)
    {
      await dbContext.Currency.AddAsync(currency);
      await dbContext.SaveChangesAsync();

      return currency;
    }

    public async Task<Currency?> DeleteAsync(Guid id)
    {
      var existingCurrency = await dbContext.Currency.FirstOrDefaultAsync(x => x.CurrencyId == id);

      if (existingCurrency == null)
      {
            return null;
      }

      // Delete the currency
      dbContext.Currency.Remove(existingCurrency);
      await dbContext.SaveChangesAsync();
      return existingCurrency;
    }

    public async Task<Currency?> UpdateAsync(Guid id, Currency currency)
    {
      var existingCurrency = await dbContext.Currency.FirstOrDefaultAsync(x => x.CurrencyId == id);

      if (existingCurrency == null)
      {
            return null;
      }

      existingCurrency.CurrencyName = currency.CurrencyName;
      await dbContext.SaveChangesAsync();
      return existingCurrency;
    }
    

  }
}