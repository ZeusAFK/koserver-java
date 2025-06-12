using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using KnightOnline.Domain.Networking; // For Packet class
using KnightOnline.Infrastructure.Security; // For Cryptor
// using Microsoft.Extensions.Logging; // Optional

namespace KnightOnline.Infrastructure.Networking
{
    public class PacketFrameReader
    {
        public const ushort ExpectedHeader = 0x55AA; // AA 55 in memory (Little Endian)
        // If header is AA 55 (Little Endian), then tail is 55 AA
        public const ushort ExpectedTail = 0xAA55;   // 55 AA in memory (Little Endian)

        // private readonly ILogger<PacketFrameReader> _logger; // Optional

        // public PacketFrameReader(ILogger<PacketFrameReader> logger)
        // {
        //     _logger = logger;
        // }
        public PacketFrameReader() {} // Constructor

        public async Task<Packet?> ReadNextPacketAsync(NetworkStream stream, Cryptor? cryptor, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Read Header (2 bytes)
                byte[] headerBuffer = new byte[2];
                int bytesRead = await ReadExactlyAsync(stream, headerBuffer, cancellationToken);
                if (bytesRead < 2) return null; // Stream ended or error

                ushort header = BitConverter.ToUInt16(headerBuffer, 0);
                if (header != ExpectedHeader)
                {
                    // _logger?.LogWarning($"Invalid packet header received: {header:X4}. Expected: {ExpectedHeader:X4}. Closing connection or attempting to resync.");
                    Console.WriteLine($"Invalid packet header received: {header:X4}"); // Placeholder
                    throw new InvalidDataException($"Invalid packet header: {header:X4}.");
                }

                // 2. Read Length (2 bytes)
                byte[] lengthBuffer = new byte[2];
                bytesRead = await ReadExactlyAsync(stream, lengthBuffer, cancellationToken);
                if (bytesRead < 2) return null;

                ushort dataLength = BitConverter.ToUInt16(lengthBuffer, 0); // Assumed Little-Endian from Java ArrayUtils

                // Basic validation for dataLength, e.g., against a max packet size
                if (dataLength == 0 || dataLength > 8192) // Max 8KB packet data, adjust as needed
                {
                    // _logger?.LogWarning($"Invalid packet data length: {dataLength}. Must be > 0 and <= 8192.");
                     Console.WriteLine($"Invalid packet data length: {dataLength}"); // Placeholder
                    throw new InvalidDataException($"Invalid packet data length: {dataLength}.");
                }

                // 3. Read Data (dataLength bytes)
                byte[] dataBuffer = new byte[dataLength];
                bytesRead = await ReadExactlyAsync(stream, dataBuffer, cancellationToken);
                if (bytesRead < dataLength) return null;

                // DECRYPTION STEP
                if (cryptor != null)
                {
                    dataBuffer = cryptor.Process(dataBuffer);
                    // After decryption, the actual data length might have changed if padding was involved
                    // and if the cryptor handles unpadding and returns a new buffer.
                    // For the current Cryptor (JvCrypt), it's an in-place XOR, so length doesn't change.
                    // If it could change, the `dataLength` variable would need to be updated,
                    // and the opcode/payload extraction below would use the new length.
                }

                // 4. Read Tail (2 bytes)
                byte[] tailBuffer = new byte[2];
                bytesRead = await ReadExactlyAsync(stream, tailBuffer, cancellationToken);
                if (bytesRead < 2) return null;

                ushort tail = BitConverter.ToUInt16(tailBuffer, 0);
                // ExpectedTail is ExpectedHeader with bytes swapped.
                // If ExpectedHeader = 0x55AA (AA 55 in memory), ExpectedTail = 0xAA55 (55 AA in memory).
                if (tail != ExpectedTail)
                {
                    // _logger?.LogWarning($"Invalid packet tail received: {tail:X4}. Expected: {ExpectedTail:X4}.");
                    Console.WriteLine($"Invalid packet tail received: {tail:X4}"); // Placeholder
                    throw new InvalidDataException($"Invalid packet tail: {tail:X4}.");
                }

                // 5. Extract Opcode and create Packet
                // Opcode is the first byte of the dataBuffer as per Java PacketListener logic
                if (dataBuffer.Length < 1) // Should be caught by dataLength == 0 check earlier
                {
                    // _logger?.LogWarning("Data buffer is empty, cannot extract opcode.");
                    Console.WriteLine("Data buffer is empty, cannot extract opcode."); // Placeholder
                    throw new InvalidDataException("Data buffer is empty, cannot extract opcode.");
                }

                ushort opcode = dataBuffer[0]; // Opcode is a single byte in the Java example

                // The actual payload for the C# Packet object will be the dataBuffer *excluding* the first byte (opcode).
                byte[] payload = new byte[dataBuffer.Length - 1];
                Array.Copy(dataBuffer, 1, payload, 0, payload.Length);

                // _logger?.LogDebug($"Successfully read packet. Opcode: {opcode}, DataLength: {dataLength-1}");
                return new Packet(opcode, payload);
            }
            catch (IOException ex) // Includes SocketException
            {
                // _logger?.LogInformation(ex, "IOException while reading packet (e.g., connection closed).");
                Console.WriteLine($"IOException reading packet: {ex.Message}"); // Placeholder
                return null; // Indicate stream is no longer readable or client disconnected
            }
            catch (ObjectDisposedException ex)
            {
                // _logger?.LogInformation(ex, "NetworkStream disposed while reading packet.");
                 Console.WriteLine($"NetworkStream disposed: {ex.Message}"); // Placeholder
                return null;
            }
            catch (OperationCanceledException)
            {
                // _logger?.LogInformation("Packet reading cancelled.");
                Console.WriteLine("Packet reading cancelled."); // Placeholder
                return null;
            }
            catch (InvalidDataException ex)
            {
                // _logger?.LogWarning(ex, "Invalid packet data encountered.");
                // This exception is thrown by this method for framing errors.
                // Depending on policy, might want to close connection here or let caller handle.
                throw; // Rethrow to allow caller (e.g. ClientSession) to handle this, possibly by closing connection.
            }
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
                    // Stream was closed prematurely by the other side.
                    break;
                }
                totalBytesRead += bytesRead;
                offset += bytesRead;
            }
            return totalBytesRead;
        }
    }
}
