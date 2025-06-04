using KnightOnline.Application.Contracts.Infrastructure;
using KnightOnline.Domain.Accounts;
using KnightOnline.Infrastructure.Data; // Updated to use AppDbContext from KnightOnline.Infrastructure.Data
using Microsoft.EntityFrameworkCore; // For DbContext and LINQ async extensions
using System.Threading.Tasks;

namespace KnightOnline.Infrastructure.Data.Repositories
{
    public class AccountRepository : IAccountRepository // Assuming a generic BaseRepository is not yet created
    {
        protected readonly AppDbContext _dbContext;

        public AccountRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account?> GetByIdAsync(UserId id)
        {
            return await _dbContext.Set<Account>().FindAsync(id);
        }

        public async Task<Account?> GetByUsernameAsync(string username)
        {
            return await _dbContext.Set<Account>().FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await _dbContext.Set<Account>().FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<IReadOnlyList<Account>> ListAllAsync()
        {
            return await _dbContext.Set<Account>().ToListAsync();
        }

        public async Task<Account> AddAsync(Account entity)
        {
            await _dbContext.Set<Account>().AddAsync(entity);
            // await _dbContext.SaveChangesAsync(); // SaveChanges would typically be part of a UnitOfWork
            return entity;
        }

        public Task UpdateAsync(Account entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            // await _dbContext.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Account entity)
        {
            _dbContext.Set<Account>().Remove(entity);
            // await _dbContext.SaveChangesAsync();
            return Task.CompletedTask;
        }
    }
}
