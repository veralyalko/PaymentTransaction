using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Repositories
{
  public interface IProviderRepository
  {
    Task<List<Provider>> GetAllAsync();
    Task<Provider?> GetByIdAsync(Guid id);
    Task<Provider> CreateAsync(Provider provider);
    Task<Provider?> DeleteAsync(Guid id);
    Task<Provider?> UpdateAsync(Guid id, Provider provider);
    Task<Provider?> GetByNameAsync(string name);

  }
}