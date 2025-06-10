using System;
using System.Linq; // For Reverse
using System.Numerics; // For BigInteger
using System.Text; // For Encoding if used, though not directly here for payload
using System.Threading.Tasks;
using KnightOnline.Application.Contracts.Networking;
using KnightOnline.Domain.Networking;
using KnightOnline.Infrastructure.Security; // For CryptorKey
// using Microsoft.Extensions.Logging; // Optional

namespace KnightOnline.LoginServer.Features.Security.Handlers
{
    public class EncryptionKeyExchangeHandler : IPacketHandler
    {
        // private readonly ILogger<EncryptionKeyExchangeHandler> _logger; // Optional

        public ushort Opcode => ClientOpcodes.EncryptionRequest; // 0x00F2

        // public EncryptionKeyExchangeHandler(ILogger<EncryptionKeyExchangeHandler> logger)
        public EncryptionKeyExchangeHandler()
        {
            // _logger = logger;
            // This handler currently has no constructor dependencies, but could if needed (e.g. a service to store active keys)
        }

        public async Task HandlePacketAsync(IClientSession session, Packet requestPacket)
        {
            // _logger?.LogInformation($"Session {session.SessionId}: Received EncryptionRequest (Opcode: {requestPacket.Opcode}).");
            Console.WriteLine($"Session {session.SessionId}: Received EncryptionRequest (Opcode: {requestPacket.Opcode})."); // Placeholder

            // Request packet for LS_CRYPTION is expected to have an empty payload
            // (after the initial opcode byte which is handled by PacketFrameReader).

            BigInteger publicKey = CryptorKey.GenerateKey();

            // TODO: Store the 'publicKey' in the IClientSession to initialize its Cryptor instance.
            // This is currently blocked by tool issues preventing IClientSession modification.
            // Example conceptual call: session.InitializeEncryption(publicKey);
            // _logger?.LogInformation($"Session {session.SessionId}: Generated public key {publicKey.ToString("x")}.");
            Console.WriteLine($"Session {session.SessionId}: Generated public key (first few bytes of hex for brevity): {publicKey.ToString("x").Substring(0, Math.Min(10, publicKey.ToString("x").Length)) }...");


            // Prepare key data for the client as per Java logic:
            // String keyString = publicKey.toString(16);
            // byte[] keyData = ArrayUtils.hexStringToByteArray(keyString);
            // keyData = ArrayUtils.byteArrayReverse(keyData);

            string keyHexString = publicKey.ToString("x"); // Lowercase hex string

            // Convert hex string to byte array. Helper from Cryptor.cs can be made public or duplicated if preferred.
            // For now, let's use Convert.FromHexString directly, ensuring padding for odd length.
            if (keyHexString.Length % 2 != 0)
            {
                keyHexString = "0" + keyHexString; // Pad with leading zero if odd length
            }
            byte[] keyData = Convert.FromHexString(keyHexString);

            Array.Reverse(keyData); // Reverse the byte array

            // _logger?.LogDebug($"Session {session.SessionId}: Sending reversed key data (hex): {Convert.ToHexString(keyData)}");
            Console.WriteLine($"Session {session.SessionId}: Sending reversed key data (first few bytes hex): {Convert.ToHexString(keyData).Substring(0, Math.Min(20, Convert.ToHexString(keyData).Length))}...");

            var responsePacket = new Packet(ServerOpcodes.EncryptionResponse); // Opcode 0x00F2
            responsePacket.WriteBytes(keyData); // Write the raw byte array

            await session.SendPacketAsync(responsePacket);
        }
    }
}
