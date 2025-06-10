using System;
using System.Threading.Tasks;
using KnightOnline.Application.Contracts.Networking;
using KnightOnline.Domain.Networking;
using KnightOnline.Infrastructure.Configuration; // For AppSettings
// using Microsoft.Extensions.Logging; // Optional

namespace KnightOnline.LoginServer.Features.System.Handlers
{
    public class VersionRequestHandler : IPacketHandler
    {
        // private readonly ILogger<VersionRequestHandler> _logger; // Optional
        private readonly AppSettings _appSettings;

        public ushort Opcode => ClientOpcodes.VersionRequest;

        // public VersionRequestHandler(ILogger<VersionRequestHandler> logger, AppSettings appSettings)
        public VersionRequestHandler(AppSettings appSettings)
        {
            // _logger = logger;
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task HandlePacketAsync(IClientSession session, Packet requestPacket)
        {
            // _logger?.LogInformation($"Session {session.SessionId}: Received VersionRequest (Opcode: {requestPacket.Opcode}).");
            Console.WriteLine($"Session {session.SessionId}: Received VersionRequest (Opcode: {requestPacket.Opcode})."); // Placeholder log

            // The requestPacket's payload is expected to be empty for this handler,
            // as the opcode itself was the first byte of the data in the frame.
            // No data needs to be read from requestPacket for this specific handler.

            string serverVersion = _appSettings.LoginServer.Version ?? "N/A";

            // _logger?.LogInformation($"Session {session.SessionId}: Sending VersionResponse with version {serverVersion}.");
            Console.WriteLine($"Session {session.SessionId}: Sending VersionResponse with version {serverVersion}."); // Placeholder log

            var responsePacket = new Packet(ServerOpcodes.VersionResponse);
            responsePacket.WriteString(serverVersion); // Assuming Packet.WriteString handles length prefixing etc.

            await session.SendPacketAsync(responsePacket);
        }
    }
}
