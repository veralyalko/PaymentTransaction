using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Repositories
{
  public interface ITransactionRepository
  {
    Task<List<Transaction>> GetAllAsync();
    Task<Transaction?> GetByIdAsync(Guid id);
    Task<Transaction> CreateAsync(Transaction transaction);
    Task<Transaction?> DeleteAsync(Guid id);
    Task<Transaction?> UpdateAsync(Guid id, Transaction transaction);

  }
}