using KnightOnline.Application.Contracts.Infrastructure;
using KnightOnline.Domain.Players;
using KnightOnline.Domain.Accounts; // For UserId
using KnightOnline.Infrastructure.Data; // For AppDbContext
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnightOnline.Infrastructure.Data.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        protected readonly AppDbContext _dbContext;

        public PlayerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Player?> GetByIdAsync(CharacterId id)
        {
            return await _dbContext.Set<Player>().FindAsync(id);
        }

        public async Task<IReadOnlyList<Player>> ListAllAsync()
        {
            return await _dbContext.Set<Player>().ToListAsync();
        }

        public async Task<Player> AddAsync(Player entity)
        {
            await _dbContext.Set<Player>().AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Player entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Player entity)
        {
            _dbContext.Set<Player>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Player>> GetByAccountIdAsync(UserId accountId)
        {
            return await _dbContext.Set<Player>()
                                 .Where(p => p.AccountId == accountId)
                                 .ToListAsync();
        }
    }
}
