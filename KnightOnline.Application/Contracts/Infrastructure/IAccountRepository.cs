using KnightOnline.Domain.Accounts; // For Account and UserId

namespace KnightOnline.Application.Contracts.Infrastructure
{
    public interface IAccountRepository : IRepository<Account, UserId>
    {
        Task<Account?> GetByUsernameAsync(string username);
        Task<Account?> GetByEmailAsync(string email);
    }
}
