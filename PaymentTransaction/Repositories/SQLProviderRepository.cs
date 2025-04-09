using Microsoft.EntityFrameworkCore;
using PaymentTransaction.Data;
using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Repositories
{
    public class SQLProviderRepository : IProviderRepository
    {
        private readonly PaymentTransactionDbContext dbContext;
        public SQLProviderRepository(PaymentTransactionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Provider>> GetAllAsync()
        {
            return await dbContext.Provider.ToListAsync();
        }

        public async Task<Provider?> GetByIdAsync(Guid id)
        {
            return await dbContext.Provider.SingleOrDefaultAsync(x => x.ProviderId == id);
        }

        public async Task<Provider?> GetByNameAsync(string name)
        {
            return await dbContext.Provider.SingleOrDefaultAsync(x => x.ProviderName == name);
        }

        public async Task<Provider> CreateAsync(Provider provider)
        {
            await dbContext.Provider.AddAsync(provider);
            await dbContext.SaveChangesAsync();

            return provider;
        }

        public async Task<Provider?> DeleteAsync(Guid id)
        {
            var existingProvider = await dbContext.Provider.FirstOrDefaultAsync(x => x.ProviderId == id);

            if (existingProvider == null)
            {
                return null;
            }

            // Delete the provider
            dbContext.Provider.Remove(existingProvider);
            await dbContext.SaveChangesAsync();
            return existingProvider;
          }

          public async Task<Provider?> UpdateAsync(Guid id, Provider provider)
          {
            var existingProvider = await dbContext.Provider.FirstOrDefaultAsync(x => x.ProviderId == id);

            if (existingProvider == null)
            {
                return null;
            }

            existingProvider.ProviderName = provider.ProviderName;
            await dbContext.SaveChangesAsync();
            return existingProvider;
        }
    }
}