using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using KnightOnline.Application.Contracts.Networking; // For IPacketHandler, IClientSession
using KnightOnline.Domain.Networking; // For Packet, ClientOpcodes, ServerOpcodes
using KnightOnline.Infrastructure.Configuration.Models; // For LoginServerSettings
using Microsoft.Extensions.Options; // For IOptions
// using Microsoft.Extensions.Logging; // Optional for logging

namespace KnightOnline.LoginServer.Features.System.Handlers
{
    public class VersionCheckHandler : IPacketHandler
    {
        public ushort Opcode => ClientOpcodes.VersionRequest;

        private readonly LoginServerSettings _loginServerSettings;
        // private readonly ILogger<VersionCheckHandler> _logger; // Optional

        public VersionCheckHandler(IOptions<LoginServerSettings> loginServerSettingsOptions /*, ILogger<VersionCheckHandler> logger = null */)
        {
            _loginServerSettings = loginServerSettingsOptions?.Value ?? throw new ArgumentNullException(nameof(loginServerSettingsOptions));
            // _logger = logger;
        }

        public async Task HandlePacketAsync(IClientSession session, Packet requestPacket)
        {
            // _logger?.LogDebug($"Handling VersionCheck request from session {session.SessionId}");
            Console.WriteLine($"Handling VersionCheck request from session {session.SessionId}"); // Placeholder

            // 1. Deserialize clientVersion from requestPacket payload
            // Payload: clientVersion (ushort, 2 bytes)
            ushort clientVersion = 0;
            byte[] requestPayload = requestPacket.GetPayload();
            if (requestPayload.Length >= 2)
            {
                clientVersion = BitConverter.ToUInt16(requestPayload, 0);
            }
            else
            {
                // _logger?.LogWarning($"Session {session.SessionId}: Received VersionCheck request with invalid payload length: {requestPayload.Length}");
                Console.WriteLine($"Session {session.SessionId}: Received VersionCheck request with invalid payload length: {requestPayload.Length}"); // Placeholder
                session.Close(); // Invalid packet, close session
                return;
            }

            // _logger?.LogInformation($"Session {session.SessionId}: Client version is {clientVersion}. Server version is {_loginServerSettings.Version}.");
            Console.WriteLine($"Session {session.SessionId}: Client version is {clientVersion}. Server version is {_loginServerSettings.Version}."); // Placeholder

            // 2. Compare versions and prepare response
            byte resultCode;
            ushort serverVersionNumeric;

            // Assuming _loginServerSettings.Version is a string like "2100". Convert to ushort.
            if (!ushort.TryParse(_loginServerSettings.Version, out serverVersionNumeric))
            {
                // _logger?.LogError($"Server version configured in LoginServerSettings ('{_loginServerSettings.Version}') is not a valid ushort.");
                Console.WriteLine($"Server version configured ('{_loginServerSettings.Version}') is not valid."); // Placeholder
                // Fallback or send error - for now, let's assume it matches client's type or send a default
                serverVersionNumeric = clientVersion; // Or some default like 0, causing a mismatch
            }

            if (clientVersion == serverVersionNumeric)
            {
                resultCode = 0x00; // Version OK
            }
            else
            {
                resultCode = 0x01; // Version Mismatch, Patch Required (default)
                // Optionally, add more sophisticated logic for resultCode 0x02 (client too old) if needed
            }

            // 3. Construct the response packet
            // Payload: resultCode (byte), serverVersion (ushort), ftpUrlLength (byte), ftpUrl (string), ftpPathLength (byte), ftpPath (string)
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms, Encoding.ASCII, leaveOpen: false)) // Using ASCII for strings as is common
            {
                writer.Write(resultCode);
                writer.Write(serverVersionNumeric);

                string ftpUrl = _loginServerSettings.FtpUrl ?? string.Empty;
                byte[] ftpUrlBytes = Encoding.ASCII.GetBytes(ftpUrl);
                writer.Write((byte)ftpUrlBytes.Length);
                writer.Write(ftpUrlBytes);

                string ftpPath = _loginServerSettings.FtpPath ?? string.Empty;
                byte[] ftpPathBytes = Encoding.ASCII.GetBytes(ftpPath);
                writer.Write((byte)ftpPathBytes.Length);
                writer.Write(ftpPathBytes);

                writer.Flush();
                byte[] responsePayload = ms.ToArray();
                var responsePacket = new Packet(ServerOpcodes.VersionResponse, responsePayload);

                // _logger?.LogInformation($"Session {session.SessionId}: Sending VersionCheck response. Result: {resultCode}, ServerVersion: {serverVersionNumeric}");
                Console.WriteLine($"Session {session.SessionId}: Sending VersionCheck response. Result: {resultCode}, ServerVersion: {serverVersionNumeric}"); // Placeholder
                await session.SendPacketAsync(responsePacket);
            }
        }
    }
}
