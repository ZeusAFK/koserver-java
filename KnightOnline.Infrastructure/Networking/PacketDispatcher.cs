using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KnightOnline.Application.Contracts.Networking;
using KnightOnline.Domain.Networking;
// using Microsoft.Extensions.Logging; // Optional

namespace KnightOnline.Infrastructure.Networking
{
    public class PacketDispatcher : IPacketDispatcher
    {
        private readonly Dictionary<ushort, IPacketHandler> _handlers = new Dictionary<ushort, IPacketHandler>();
        // private readonly ILogger<PacketDispatcher> _logger; // Optional
        // private readonly IServiceProvider _serviceProvider; // For resolving scoped handlers

        // public PacketDispatcher(ILogger<PacketDispatcher> logger, IServiceProvider serviceProvider)
        public PacketDispatcher(/* ILogger<PacketDispatcher> logger */)
        {
            // _logger = logger;
            // _serviceProvider = serviceProvider; // If handlers are registered in DI
        }

        public void RegisterHandler(IPacketHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            if (_handlers.ContainsKey(handler.Opcode))
            {
                // _logger?.LogWarning($"Handler for opcode {handler.Opcode} is already registered. Replacing.");
                Console.WriteLine($"Handler for opcode {handler.Opcode} is already registered. Replacing."); // Placeholder
            }
            _handlers[handler.Opcode] = handler;
            // _logger?.LogInformation($"Registered handler for opcode {handler.Opcode}: {handler.GetType().Name}");
            Console.WriteLine($"Registered handler for opcode {handler.Opcode}: {handler.GetType().Name}");
        }

        public async Task DispatchPacketAsync(IClientSession session, Packet packet)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            if (packet == null) throw new ArgumentNullException(nameof(packet));

            if (_handlers.TryGetValue(packet.Opcode, out IPacketHandler handler))
            {
                try
                {
                    // _logger?.LogDebug($"Dispatching opcode {packet.Opcode} to handler {handler.GetType().Name} for session {session.SessionId}");
                    await handler.HandlePacketAsync(session, packet);
                }
                catch (Exception ex)
                {
                    // _logger?.LogError(ex, $"Error handling packet with opcode {packet.Opcode} by handler {handler.GetType().Name} for session {session.SessionId}");
                    // Optionally, close session or send error packet
                    Console.WriteLine($"Error handling packet {packet.Opcode}: {ex.Message}"); // Placeholder
                }
            }
            else
            {
                // _logger?.LogWarning($"No handler registered for opcode {packet.Opcode} from session {session.SessionId}. Packet ignored.");
                Console.WriteLine($"No handler for opcode {packet.Opcode}. Packet ignored."); // Placeholder
            }
        }
    }
}
