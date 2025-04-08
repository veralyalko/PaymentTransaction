using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Repositories
{
  public interface IStatusRepository
  {
    Task<List<Status>> GetAllAsync();
    Task<Status?> GetByIdAsync(Guid id);
    Task<Status> CreateAsync(Status status);
    Task<Status?> DeleteAsync(Guid id);
    Task<Status?> UpdateAsync(Guid id, Status status);

  }
}