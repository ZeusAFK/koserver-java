using KnightOnline.Domain.Players; // For Player and CharacterId
using KnightOnline.Domain.Accounts; // For UserId

namespace KnightOnline.Application.Contracts.Infrastructure
{
    public interface IPlayerRepository : IRepository<Player, CharacterId>
    {
        Task<IEnumerable<Player>> GetByAccountIdAsync(UserId accountId);
    }
}
