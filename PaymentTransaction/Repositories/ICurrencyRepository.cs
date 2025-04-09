using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Repositories
{
    public interface ICurrencyRepository
    {
        Task<List<Currency>> GetAllAsync();
        Task<Currency?> GetByIdAsync(Guid id);
        Task<Currency> CreateAsync(Currency currency);
        Task<Currency?> DeleteAsync(Guid id);
        Task<Currency?> UpdateAsync(Guid id, Currency currency);

    }
}