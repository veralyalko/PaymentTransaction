using Microsoft.EntityFrameworkCore;
using PaymentTransaction.Data;
using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Repositories
{
    public class SQLStatusRepository : IStatusRepository
    {
        private readonly PaymentTransactionDbContext dbContext;
        public SQLStatusRepository(PaymentTransactionDbContext dbContext)
        {
           this.dbContext = dbContext;
        }
        public async Task<List<Status>> GetAllAsync()
        {
            return await dbContext.Status.ToListAsync();
        }

        public async Task<Status?> GetByIdAsync(Guid id)
        {
            return await dbContext.Status.SingleOrDefaultAsync(x => x.StatusId == id);
        }

        public async Task<Status> CreateAsync(Status status)
        {
            await dbContext.Status.AddAsync(status);
            await dbContext.SaveChangesAsync();

            return status;
        }

        public async Task<Status?> DeleteAsync(Guid id)
        {
            var existingStatus = await dbContext.Status.FirstOrDefaultAsync(x => x.StatusId == id);

            if (existingStatus == null)
            {
                return null;
            }

            // Delete the status
            dbContext.Status.Remove(existingStatus);
            await dbContext.SaveChangesAsync();
            return existingStatus;
        }

        public async Task<Status?> UpdateAsync(Guid id, Status status)
        {
            var existingStatus = await dbContext.Status.FirstOrDefaultAsync(x => x.StatusId == id);

            if (existingStatus == null)
            {
                return null;
            }

            existingStatus.StatusName = status.StatusName;
            await dbContext.SaveChangesAsync();
            return existingStatus;
        }
    }
}