using System;
using System.Threading.Tasks;
using KnightOnline.Application.Contracts.Networking;
using KnightOnline.Domain.Networking;
using KnightOnline.Application.Features.Authentication.Commands; // For LoginCommand
using KnightOnline.Application.DTOs.Authentication;          // For LoginResultDto
using KnightOnline.Domain.Enums;                              // For LoginResult enum
using MediatR;
// using Microsoft.Extensions.Logging; // Optional

namespace KnightOnline.LoginServer.Features.Authentication.Handlers
{
    public class LoginRequestHandler : IPacketHandler
    {
        private readonly IMediator _mediator;
        // private readonly ILogger<LoginRequestHandler> _logger; // Optional

        public ushort Opcode => ClientOpcodes.LoginRequest;

        // public LoginRequestHandler(IMediator mediator, ILogger<LoginRequestHandler> logger)
        public LoginRequestHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            // _logger = logger;
        }

        public async Task HandlePacketAsync(IClientSession session, Packet requestPacket)
        {
            // _logger?.LogInformation($"Session {session.SessionId}: Received LoginRequest (Opcode: {requestPacket.Opcode}).");
            Console.WriteLine($"Session {session.SessionId}: Received LoginRequest (Opcode: {requestPacket.Opcode})."); // Placeholder

            string username;
            string password;

            try
            {
                username = requestPacket.ReadString(); // Reads length-prefixed string
                password = requestPacket.ReadString(); // Reads another length-prefixed string
                // _logger?.LogDebug($"Session {session.SessionId}: Parsed Username '{username}', Password '***'.");
                Console.WriteLine($"Session {session.SessionId}: Parsed Username '{username}'."); // Placeholder
            }
            catch (Exception ex) // Could be EndOfStreamException or other ReadBytes issues
            {
                // _logger?.LogError(ex, $"Session {session.SessionId}: Error reading username/password from login packet.");
                Console.WriteLine($"Session {session.SessionId}: Error reading login packet. {ex.Message}"); // Placeholder
                session.Close(); // Close session on packet format error
                return;
            }

            var loginCommand = new LoginCommand(username, password);
            LoginResultDto loginResult = await _mediator.Send(loginCommand);

            // TODO: Update IClientSession state based on loginResult.
            // For example:
            // if (loginResult.IsSuccess)
            // {
            //    session.SetAuthenticated(loginResult.Username, accountIdFromDtoOrLookup);
            //    // IClientSession would need SetAuthenticated method and properties for Username/AccountId.
            // }
            // _logger?.LogInformation($"Session {session.SessionId}: Login attempt for '{username}' result: {loginResult.ResultCode}.");
            Console.WriteLine($"Session {session.SessionId}: Login attempt for '{username}' result: {loginResult.ResultCode}."); // Placeholder

            // Construct and send response packet
            var responsePacket = new Packet(ServerOpcodes.LoginResponse); // Uses LS_LOGIN (0xF3) opcode
            responsePacket.WriteByte((byte)loginResult.ResultCode); // Result code
            responsePacket.WriteShort(loginResult.FixedValue);     // The hardcoded 7857
            responsePacket.WriteString(loginResult.Username);      // Original username

            await session.SendPacketAsync(responsePacket);

            // If login failed and requires disconnect (e.g. AuthBanned, AuthInvalid too many times), close session.
            if (!loginResult.IsSuccess &&
                (loginResult.ResultCode == LoginResult.AuthBanned || loginResult.ResultCode == LoginResult.AuthNotFound || loginResult.ResultCode == LoginResult.AuthInvalid))
            {
                // _logger?.LogInformation($"Session {session.SessionId}: Login failed ({loginResult.ResultCode}), closing session for user '{username}'.");
                Console.WriteLine($"Session {session.SessionId}: Login failed ({loginResult.ResultCode}), closing session for user '{username}'."); // Placeholder
                session.Close();
            }
        }
    }
}
