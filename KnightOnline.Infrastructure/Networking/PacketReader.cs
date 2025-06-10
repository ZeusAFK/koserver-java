using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using KnightOnline.Domain.Networking; // For Packet
// using Microsoft.Extensions.Logging; // Optional

namespace KnightOnline.Infrastructure.Networking
{
    public class PacketReadingException : Exception
    {
        public PacketReadingException(string message) : base(message) { }
        public PacketReadingException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class PacketReader
    {
        // private readonly ILogger<PacketReader> _logger; // Optional

        public const ushort ExpectedHeader = 0x55AA; // AA 55 in Little-Endian
        public const ushort ExpectedTail = 0xAA55;   // 55 AA in Little-Endian (inverted header bytes)

        // public PacketReader(ILogger<PacketReader> logger)
        // {
        //     _logger = logger;
        // }
        public PacketReader() {}


        public async Task<Packet?> ReadNextPacketAsync(NetworkStream stream, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Read Header (2 bytes)
                byte[] headerBuffer = new byte[2];
                int bytesRead = await ReadExactlyAsync(stream, headerBuffer, cancellationToken);
                if (bytesRead < 2)
                {
                    // _logger?.LogDebug("Connection closed or stream ended while reading packet header.");
                    return null; // Stream ended
                }

                ushort header = BitConverter.ToUInt16(headerBuffer, 0); // Assumes Little-Endian
                if (header != ExpectedHeader)
                {
                    // _logger?.LogWarning($"Invalid packet header received: 0x{header:X4}. Expected 0x{ExpectedHeader:X4}.");
                    throw new PacketReadingException($"Invalid packet header: 0x{header:X4}");
                }

                // 2. Read Length (2 bytes)
                byte[] lengthBuffer = new byte[2];
                await ReadExactlyAsync(stream, lengthBuffer, cancellationToken);
                ushort dataLength = BitConverter.ToUInt16(lengthBuffer, 0); // Assumes Little-Endian

                // Basic validation for dataLength (e.g., against a max packet size)
                // The Java code checked against Short.MAX_VALUE, which is 32767
                if (dataLength == 0) {
                     // _logger?.LogWarning("Received packet with zero data length. This might be valid (keep-alive) or an issue.");
                     // Potentially handle zero-length packets if they are part of the protocol (e.g. keep-alive)
                     // For now, assume data must include at least the opcode byte.
                     throw new PacketReadingException("Packet data length is zero. Opcode byte is missing.");
                }
                if (dataLength > 32767) // Max packet size check
                {
                    // _logger?.LogError($"Packet size {dataLength} exceeds maximum allowed (32767).");
                    throw new PacketReadingException($"Packet size {dataLength} exceeds maximum allowed.");
                }


                // 3. Read Data (dataLength bytes)
                byte[] dataBuffer = new byte[dataLength];
                await ReadExactlyAsync(stream, dataBuffer, cancellationToken);

                // 4. Read Tail (2 bytes)
                byte[] tailBuffer = new byte[2];
                await ReadExactlyAsync(stream, tailBuffer, cancellationToken);
                ushort tail = BitConverter.ToUInt16(tailBuffer, 0); // Assumes Little-Endian

                if (tail != ExpectedTail)
                {
                    // _logger?.LogWarning($"Invalid packet tail received: 0x{tail:X4}. Expected 0x{ExpectedTail:X4}.");
                    throw new PacketReadingException($"Invalid packet tail: 0x{tail:X4}");
                }

                // 5. Extract Opcode and actual payload
                // Opcode is the first byte of the dataBuffer
                if (dataBuffer.Length < 1) // Should have been caught by dataLength == 0 check
                {
                     throw new PacketReadingException("Packet data buffer is empty, cannot extract opcode.");
                }
                ushort opcode = dataBuffer[0]; // Java code used first byte as short, implies opcodes are 0-255

                byte[] payload = new byte[dataBuffer.Length - 1];
                Array.Copy(dataBuffer, 1, payload, 0, dataBuffer.Length - 1);

                return new Packet(opcode, payload);
            }
            catch (IOException ex) // Includes SocketException, EndOfStreamException
            {
                // _logger?.LogInformation(ex, "IOException during packet reading (e.g., connection closed).");
                Console.WriteLine($"IOException during packet reading: {ex.Message}"); // Placeholder
                return null; // Connection closed or error
            }
            catch (PacketReadingException ex)
            {
                // _logger?.LogWarning(ex, "Packet reading error.");
                 Console.WriteLine($"Packet reading error: {ex.Message}"); // Placeholder
                throw; // Rethrow specific packet reading errors to be handled by caller
            }
            catch (OperationCanceledException)
            {
                // _logger?.LogInformation("Packet reading was canceled.");
                Console.WriteLine("Packet reading was canceled."); // Placeholder
                return null;
            }
            // Catch other critical exceptions if necessary, but often better to let them propagate if unexpected.
        }

        private async Task<int> ReadExactlyAsync(NetworkStream stream, byte[] buffer, CancellationToken cancellationToken)
        {
            int totalBytesRead = 0;
            int offset = 0;
            while (offset < buffer.Length)
            {
                cancellationToken.ThrowIfCancellationRequested();
                int bytesRead = await stream.ReadAsync(buffer, offset, buffer.Length - offset, cancellationToken);
                if (bytesRead == 0)
                {
                    // Stream ended prematurely
                    break;
                }
                offset += bytesRead;
                totalBytesRead += bytesRead;
            }
            if (totalBytesRead < buffer.Length && totalBytesRead > 0) {
                 // This case means stream ended before we could read all requested bytes for this segment (header/length/data/tail)
                 throw new EndOfStreamException($"Stream ended prematurely. Expected {buffer.Length} bytes for current segment, but got {totalBytesRead}.");
            }
            return totalBytesRead;
        }
    }
}
