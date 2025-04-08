using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Repositories
{
  public interface IPaymentMethodRepository
  {
    Task<List<PaymentMethod>> GetAllAsync();
    Task<PaymentMethod?> GetByIdAsync(Guid id);
    Task<PaymentMethod> CreateAsync(PaymentMethod paymentMethos);
    Task<PaymentMethod?> DeleteAsync(Guid id);
    Task<PaymentMethod?> UpdateAsync(Guid id, PaymentMethod paymentMethod);

  }
}