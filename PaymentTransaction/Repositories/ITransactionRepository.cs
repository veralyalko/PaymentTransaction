using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Repositories
{
  public interface ITransactionRepository
  {
    Task<Transaction> CreateAsync(Transaction transaction);
    Task<List<Transaction>> GetAllAsync();
    Task<Transaction?> GetByIdAsync(Guid id);
    Task<Transaction?> DeleteAsync(Guid id);
    Task<Transaction?> UpdateAsync(Guid id, Transaction transaction);
    Task<Transaction?> CreateForProviderAsync(string providerName, Transaction transaction);

  }
}