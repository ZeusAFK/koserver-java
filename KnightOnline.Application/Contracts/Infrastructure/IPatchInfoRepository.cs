using KnightOnline.Domain.Client; // For PatchInfo
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnightOnline.Application.Contracts.Infrastructure
{
    public interface IPatchInfoRepository
    {
        Task<PatchInfo?> GetLatestPatchAsync();
        Task<IEnumerable<PatchInfo>> GetAllPatchesAsync(); // If multiple patches are managed
    }
}
