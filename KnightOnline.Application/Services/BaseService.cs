using System;
using System.Threading; // For Interlocked
using System.Threading.Tasks;
using KnightOnline.Application.Contracts.Services;
// using Microsoft.Extensions.Logging; // Optional

namespace KnightOnline.Application.Services
{
    public abstract class BaseService : IServiceLifecycle
    {
        // private readonly ILogger _logger; // Optional logger
        private int _startedStatus = 0; // 0 = stopped, 1 = started

        public bool IsStarted => _startedStatus == 1;

        // protected BaseService(ILogger logger) // Example constructor with logger
        // {
        //     _logger = logger;
        // }
        protected BaseService() {}


        public async Task StartAsync() // Changed name for clarity and async
        {
            if (Interlocked.CompareExchange(ref _startedStatus, 1, 0) == 0)
            {
                // _logger?.LogInformation($"Service {this.GetType().Name} starting...");
                Console.WriteLine($"Service {this.GetType().Name} starting..."); // Placeholder
                try
                {
                    await OnInitAsync();
                    // _logger?.LogInformation($"Service {this.GetType().Name} started successfully.");
                    Console.WriteLine($"Service {this.GetType().Name} started successfully."); // Placeholder
                }
                catch (Exception ex)
                {
                    // _logger?.LogError(ex, $"Error during OnInitAsync for service {this.GetType().Name}. Reverting started status.");
                    Console.WriteLine($"Error during OnInitAsync for service {this.GetType().Name}: {ex.Message}"); // Placeholder
                    Interlocked.Exchange(ref _startedStatus, 0); // Revert status on error
                    throw;
                }
            }
            else
            {
                // _logger?.LogDebug($"Service {this.GetType().Name} is already started or starting.");
                Console.WriteLine($"Service {this.GetType().Name} is already started or starting."); // Placeholder
            }
        }

        public async Task StopAsync() // Changed name for clarity and async
        {
            if (Interlocked.CompareExchange(ref _startedStatus, 0, 1) == 1)
            {
                // _logger?.LogInformation($"Service {this.GetType().Name} stopping...");
                Console.WriteLine($"Service {this.GetType().Name} stopping..."); // Placeholder
                try
                {
                    await OnDestroyAsync();
                    // _logger?.LogInformation($"Service {this.GetType().Name} stopped successfully.");
                    Console.WriteLine($"Service {this.GetType().Name} stopped successfully."); // Placeholder
                }
                catch (Exception ex)
                {
                    // _logger?.LogError(ex, $"Error during OnDestroyAsync for service {this.GetType().Name}. Service might not be fully stopped cleanly.");
                         Console.WriteLine($"Error during OnDestroyAsync for service {this.GetType().Name}: {ex.Message}"); // Placeholder
                        // Depending on policy, you might still consider it "stopped" or in an error state.
                        // For this example, _startedStatus remains 0.
                        throw;
                }
            }
            else
            {
                // _logger?.LogDebug($"Service {this.GetType().Name} is already stopped or stopping.");
                Console.WriteLine($"Service {this.GetType().Name} is already stopped or stopping."); // Placeholder
            }
        }

        public async Task RestartAsync() // Changed name for clarity and async
        {
            await StopAsync();
            await StartAsync();
        }

        // Abstract methods to be implemented by derived classes
        public abstract Task OnInitAsync();
        public abstract Task OnDestroyAsync();
    }
}
