using KnightOnline.Domain.Client; // For ServerDetail
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnightOnline.Application.Contracts.Infrastructure
{
    public interface IServerDetailRepository
    {
        Task<ServerDetail?> GetByIdAsync(int serverId);
        Task<IEnumerable<ServerDetail>>GetAllServersAsync();
        // Potentially methods to update server status like user count if this repo manages that
    }
}
