using System;
using System.Threading.Tasks;
using KnightOnline.Application.Contracts.Networking;
using KnightOnline.Domain.Networking;
using KnightOnline.Application.Features.Server.Queries; // For GetServerListQuery
using KnightOnline.Application.DTOs.Server;          // For ServerInfoDto
using MediatR;
// using Microsoft.Extensions.Logging; // Optional

namespace KnightOnline.LoginServer.Features.Server.Handlers
{
    public class ServerListRequestHandler : IPacketHandler
    {
        private readonly IMediator _mediator;
        // private readonly ILogger<ServerListRequestHandler> _logger; // Optional

        public ushort Opcode => ClientOpcodes.ServerListRequest; // 0x00F5

        // public ServerListRequestHandler(IMediator mediator, ILogger<ServerListRequestHandler> logger)
        public ServerListRequestHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            // _logger = logger;
        }

        public async Task HandlePacketAsync(IClientSession session, Packet requestPacket)
        {
            // _logger?.LogInformation($"Session {session.SessionId}: Received ServerListRequest (Opcode: {requestPacket.Opcode}).");
            Console.WriteLine($"Session {session.SessionId}: Received ServerListRequest (Opcode: {requestPacket.Opcode})."); // Placeholder

            short echo;
            try
            {
                echo = requestPacket.ReadShort();
                // _logger?.LogDebug($"Session {session.SessionId}: Parsed echo value: {echo}.");
                Console.WriteLine($"Session {session.SessionId}: Parsed echo value: {echo}."); // Placeholder
            }
            catch (Exception ex) // Could be EndOfStreamException
            {
                // _logger?.LogError(ex, $"Session {session.SessionId}: Error reading echo from server list request packet.");
                Console.WriteLine($"Session {session.SessionId}: Error reading echo from server list request. {ex.Message}"); // Placeholder
                session.Close(); // Close session on packet format error
                return;
            }

            var getServerListQuery = new GetServerListQuery(echo);
            GetServerListQuery.Response serverListResponse = await _mediator.Send(getServerListQuery);

            var responsePacket = new Packet(ServerOpcodes.ServerListResponse); // Opcode 0x00F5

            responsePacket.WriteShort(serverListResponse.Echo);
            responsePacket.WriteByte((byte)(serverListResponse.Servers?.Count ?? 0));

            if (serverListResponse.Servers != null)
            {
                foreach (var serverInfo in serverListResponse.Servers)
                {
                    responsePacket.WriteString(serverInfo.Ip);
                    responsePacket.WriteString(serverInfo.Name);
                    responsePacket.WriteShort(serverInfo.UserCount);
                    responsePacket.WriteShort(serverInfo.ServerId); // Already short in DTO
                    responsePacket.WriteShort(serverInfo.Category);
                    responsePacket.WriteShort(serverInfo.UserMax);
                    responsePacket.WriteShort(serverInfo.UserMaxFree);
                    responsePacket.WriteByte(0); // The constant zero byte

                    // Four empty strings for KingName and Notices
                    responsePacket.WriteString(""); // King Name
                    responsePacket.WriteString(""); // Notice 1
                    responsePacket.WriteString(""); // Notice 2
                    responsePacket.WriteString(""); // Notice 3
                }
            }

            // _logger?.LogInformation($"Session {session.SessionId}: Sending ServerListResponse with {serverListResponse.Servers?.Count ?? 0} servers.");
            Console.WriteLine($"Session {session.SessionId}: Sending ServerListResponse with {serverListResponse.Servers?.Count ?? 0} servers."); // Placeholder
            await session.SendPacketAsync(responsePacket);
        }
    }
}
