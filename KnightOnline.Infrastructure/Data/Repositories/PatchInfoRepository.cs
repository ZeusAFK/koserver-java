using KnightOnline.Application.Contracts.Infrastructure;
using KnightOnline.Domain.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnightOnline.Infrastructure.Data.Repositories
{
    public class PatchInfoRepository : IPatchInfoRepository
    {
        // This would typically interact with a database or a configuration file
        private static readonly List<PatchInfo> _patches = new List<PatchInfo>
        {
            new PatchInfo("2100", "http://example.com/patch/2100.zip", "Latest game client files."),
            new PatchInfo("2099", "http://example.com/patch/2099.zip", "Previous version.")
        };

        public Task<PatchInfo?> GetLatestPatchAsync()
        {
            // Example: returning the one with the highest version number
            // In a real scenario, this might be a specific query or a designated "latest" flag.
            var latest = _patches.OrderByDescending(p => p.Version).FirstOrDefault();
            return Task.FromResult(latest);
        }

        public Task<IEnumerable<PatchInfo>> GetAllPatchesAsync()
        {
            return Task.FromResult<IEnumerable<PatchInfo>>(_patches);
        }
    }
}
