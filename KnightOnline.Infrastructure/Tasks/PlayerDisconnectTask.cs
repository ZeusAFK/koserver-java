using KnightOnline.Application.Contracts.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KnightOnline.Infrastructure.Tasks
{
    public class PlayerDisconnectTask : IBackgroundTask
    {
        private readonly int _playerId; // Example: Task might be specific to a player

        public PlayerDisconnectTask(int playerId)
        {
            _playerId = playerId;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // TODO: Implement player disconnection logic
            // For example:
            // 1. Save player data
            // 2. Clean up player session
            // 3. Notify other game systems if necessary
            Console.WriteLine($"Executing player disconnect task for player ID: {_playerId}");

            // Simulate work
            return Task.Delay(1000, cancellationToken);
        }
    }
}
