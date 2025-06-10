using System.Threading.Tasks;

namespace KnightOnline.Application.Contracts.Services
{
    public interface IServiceLifecycle
    {
        Task OnInitAsync(); // Changed to async
        Task OnDestroyAsync(); // Changed to async
    }
}
