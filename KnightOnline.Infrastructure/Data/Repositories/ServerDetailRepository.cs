using KnightOnline.Application.Contracts.Infrastructure;
using KnightOnline.Domain.Client;
using KnightOnline.Infrastructure.Configuration.Models; // For LoginServerSettings and ServerConfigItem
using Microsoft.Extensions.Options; // For IOptions
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnightOnline.Infrastructure.Data.Repositories
{
    public class ServerDetailRepository : IServerDetailRepository
    {
        private readonly LoginServerSettings _loginServerSettings;

        public ServerDetailRepository(IOptions<LoginServerSettings> loginServerSettingsOptions)
        {
            _loginServerSettings = loginServerSettingsOptions.Value;
        }

        public Task<ServerDetail?> GetByIdAsync(int serverId)
        {
            var serverConfigItem = _loginServerSettings.ServerList?.FirstOrDefault(s => s.Id == serverId);

            if (serverConfigItem == null)
            {
                return Task.FromResult<ServerDetail?>(null);
            }

            var serverDetail = ServerDetail.Create(
                serverConfigItem.Id,
                serverConfigItem.Name,
                serverConfigItem.IpAddress,
                serverConfigItem.Port,
                serverConfigItem.MaxUsers,
                serverConfigItem.UserMaxFree, // Added
                serverConfigItem.Category
            );
            // Note: UserCount in ServerConfigItem is likely a default/initial value.
            // serverDetail.UpdateCurrentUserCount(serverConfigItem.UserCount); // CurrentUsers is initialized to 0 by ServerDetail.Create

            return Task.FromResult<ServerDetail?>(serverDetail);
        }

        public Task<IEnumerable<ServerDetail>> GetAllServersAsync()
        {
            if (_loginServerSettings.ServerList == null || !_loginServerSettings.ServerList.Any())
            {
                return Task.FromResult(Enumerable.Empty<ServerDetail>());
            }

            var serverDetails = _loginServerSettings.ServerList.Select(serverConfigItem =>
                {
                    var detail = ServerDetail.Create(
                        serverConfigItem.Id,
                        serverConfigItem.Name,
                        serverConfigItem.IpAddress,
                        serverConfigItem.Port,
                        serverConfigItem.MaxUsers,
                        serverConfigItem.UserMaxFree, // Added
                        serverConfigItem.Category
                    );
                    // detail.UpdateCurrentUserCount(serverConfigItem.UserCount); // CurrentUsers is initialized to 0 by ServerDetail.Create
                    return detail;
                }
            ).ToList();

            return Task.FromResult<IEnumerable<ServerDetail>>(serverDetails);
        }

        /*
        public Task UpdateUserCountAsync(int serverId, int userCount)
        {
            var serverConfigItem = _loginServerSettings.ServerList?.FirstOrDefault(s => s.Id == serverId);
            if (serverConfigItem != null)
            {
                // serverConfigItem.UserCount = userCount;
            }
            return Task.CompletedTask;
        }
        */
    }
}
