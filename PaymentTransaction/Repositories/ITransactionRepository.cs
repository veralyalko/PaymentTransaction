using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Repositories
{
  public interface ITransactionRepository
  {
    Task<Transaction> CreateAsync(Transaction transaction);
    // Task<List<Transaction>> GetAllAsync(string? filterOn = null, string? filterQuery = null);

    Task<List<Transaction>> GetAllAsync(
    string? filterOn = null,
    string? filterQuery = null,
    DateTime? fromDate = null,
    DateTime? toDate = null);

    Task<Transaction?> GetByIdAsync(Guid id);
    Task<Transaction?> DeleteAsync(Guid id);
    Task<Transaction?> UpdateAsync(Guid id, Transaction transaction);
    Task<Transaction?> CreateForProviderAsync(string providerName, Transaction transaction);
    Task<TransactionSummaryDto> GetSummaryAsync();

  }
}