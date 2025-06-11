using KnightOnline.Application.Contracts.Infrastructure;
using KnightOnline.Domain.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnightOnline.Infrastructure.Data.Repositories
{
    public class ServerDetailRepository : IServerDetailRepository
    {
        // Placeholder: In-memory list. Replace with actual data access (e.g., DbContext).
        private static readonly List<ServerDetail> _serverDetails = new List<ServerDetail>
        {
            ServerDetail.Create(1, "Ares", "127.0.0.1", 15001, 1000, "Normal"),
            ServerDetail.Create(2, "Diez", "127.0.0.1", 15002, 1000, "Normal"),
            ServerDetail.Create(3, "Gordion", "127.0.0.1", 15003, 1200, "Premium")
        };

        public Task<ServerDetail?> GetByIdAsync(int serverId)
        {
            var server = _serverDetails.FirstOrDefault(s => s.Id == serverId);
            return Task.FromResult(server);
        }

        public Task<IEnumerable<ServerDetail>> GetAllServersAsync()
        {
            return Task.FromResult<IEnumerable<ServerDetail>>(_serverDetails);
        }

        // Example of how user count might be updated if this repository handles it
        public Task UpdateUserCountAsync(int serverId, int userCount)
        {
            var server = _serverDetails.FirstOrDefault(s => s.Id == serverId);
            if (server != null)
            {
                server.UpdateCurrentUserCount(userCount);
            }
            return Task.CompletedTask;
        }
    }
}
