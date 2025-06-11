using System.Threading;
using System.Threading.Tasks;

namespace KnightOnline.Application.Contracts.Tasks
{
    public interface IBackgroundTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
