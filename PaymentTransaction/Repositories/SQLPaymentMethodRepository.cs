using Microsoft.EntityFrameworkCore;
using PaymentTransaction.Data;
using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Repositories
{
    public class SQLPaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly PaymentTransactionDbContext dbContext;
        public SQLPaymentMethodRepository(PaymentTransactionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<PaymentMethod>> GetAllAsync()
        {
            return await dbContext.PaymentMethod.ToListAsync();
        }

        public async Task<PaymentMethod?> GetByIdAsync(Guid id)
        {
            return await dbContext.PaymentMethod.SingleOrDefaultAsync(x => x.PaymentMethodId == id);
        }

        public async Task<PaymentMethod> CreateAsync(PaymentMethod paymentMethod)
        {
            await dbContext.PaymentMethod.AddAsync(paymentMethod);
            await dbContext.SaveChangesAsync();

            return paymentMethod;
        }

        public async Task<PaymentMethod?> DeleteAsync(Guid id)
        {
            var existingPaymentMethod = await dbContext.PaymentMethod.FirstOrDefaultAsync(x => x.PaymentMethodId == id);

            if (existingPaymentMethod == null)
            {
                return null;
            }

            // Delete the PaymentMethod
            dbContext.PaymentMethod.Remove(existingPaymentMethod);
            await dbContext.SaveChangesAsync();
            return existingPaymentMethod;
        }

        public async Task<PaymentMethod?> UpdateAsync(Guid id, PaymentMethod paymentMethod)
        {
            var existingPaymentMethod = await dbContext.PaymentMethod.FirstOrDefaultAsync(x => x.PaymentMethodId == id);

            if (existingPaymentMethod == null)
            {
                return null;
            }

            existingPaymentMethod.PaymentMethodName = paymentMethod.PaymentMethodName;
            await dbContext.SaveChangesAsync();
            return existingPaymentMethod;
        }
    }
}