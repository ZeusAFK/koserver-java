using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using KnightOnline.Application.Contracts.Networking;
using KnightOnline.Domain.Networking;
// using Microsoft.Extensions.Logging; // Optional

namespace KnightOnline.Infrastructure.Networking
{
    public class ClientSession : IClientSession
    {
        private readonly TcpClient _tcpClient;
        private readonly NetworkStream _networkStream;
        private readonly IPacketDispatcher _packetDispatcher;
        private readonly PacketFrameReader _packetFrameReader;
        // private readonly ILogger<ClientSession> _logger; // Optional
        private readonly CancellationTokenSource _sessionCts;

        public string SessionId { get; }

        // public ClientSession(TcpClient tcpClient, IPacketDispatcher packetDispatcher, PacketFrameReader packetFrameReader, ILogger<ClientSession> logger)
        public ClientSession(TcpClient tcpClient, IPacketDispatcher packetDispatcher, PacketFrameReader packetFrameReader)
        {
            _tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            _networkStream = _tcpClient.GetStream();
            _packetDispatcher = packetDispatcher ?? throw new ArgumentNullException(nameof(packetDispatcher));
            _packetFrameReader = packetFrameReader ?? throw new ArgumentNullException(nameof(packetFrameReader));
            // _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            SessionId = Guid.NewGuid().ToString(); // Simple unique ID for the session
            _sessionCts = new CancellationTokenSource();

            // _logger.LogInformation($"Session {SessionId} created for client {tcpClient.Client.RemoteEndPoint}");
            Console.WriteLine($"Session {SessionId} created for client {tcpClient.Client.RemoteEndPoint}"); // Placeholder
        }

        public async Task ProcessIncomingDataAsync(CancellationToken linkedExternalToken = default)
        {
            // Link the external token (e.g., from server shutdown) with the session's own token
            using (var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(_sessionCts.Token, linkedExternalToken))
            {
                var cancellationToken = combinedCts.Token;
                try
                {
                    while (_tcpClient.Connected && !cancellationToken.IsCancellationRequested)
                    {
                        Packet? packet = await _packetFrameReader.ReadNextPacketAsync(_networkStream, cancellationToken);
                        if (packet != null)
                        {
                            // _logger.LogDebug($"Session {SessionId} received packet with opcode {packet.Opcode}. Dispatching...");
                             Console.WriteLine($"Session {SessionId} received packet with opcode {packet.Opcode}. Dispatching...");// Placeholder
                            await _packetDispatcher.DispatchPacketAsync(this, packet);
                        }
                        else if (!cancellationToken.IsCancellationRequested)
                        {
                            // ReadNextPacketAsync returning null (and no cancellation) usually means graceful remote close or unrecoverable stream error
                            // _logger.LogInformation($"Session {SessionId}: Remote client closed connection or stream error.");
                            Console.WriteLine($"Session {SessionId}: Remote client closed connection or stream error."); // Placeholder
                            break;
                        }
                    }
                }
                catch (InvalidDataException ex)
                {
                    // _logger.LogError(ex, $"Session {SessionId}: Invalid data received. Closing session.");
                    Console.WriteLine($"Session {SessionId}: Invalid data received. {ex.Message}. Closing session."); // Placeholder
                    // Close session due to protocol error
                }
                catch (IOException ex)
                {
                    // _logger.LogInformation(ex, $"Session {SessionId}: IOException. Likely connection issue. Closing session.");
                    Console.WriteLine($"Session {SessionId}: IOException. {ex.Message}. Closing session."); // Placeholder
                    // Connection issue
                }
                catch (OperationCanceledException)
                {
                    // _logger.LogInformation($"Session {SessionId}: Processing cancelled.");
                    Console.WriteLine($"Session {SessionId}: Processing cancelled."); // Placeholder
                    // Expected during shutdown or if session is explicitly closed
                }
                catch (Exception ex)
                {
                    // _logger.LogError(ex, $"Session {SessionId}: Unhandled exception in ProcessIncomingDataAsync. Closing session.");
                     Console.WriteLine($"Session {SessionId}: Unhandled exception. {ex.Message}. Closing session."); // Placeholder
                    // Catch-all for safety
                }
                finally
                {
                    Close();
                }
            }
        }

        public async Task SendPacketAsync(Packet packet)
        {
            if (!_tcpClient.Connected)
            {
                // _logger.LogWarning($"Session {SessionId}: Cannot send packet. Client not connected.");
                Console.WriteLine($"Session {SessionId}: Cannot send packet. Client not connected."); // Placeholder
                return;
            }

            try
            {
                // Frame the packet before sending: Header + Length + (Opcode as first byte of data + Payload) + Tail
                byte[] payload = packet.GetPayload(); // This payload already excludes the opcode byte based on PacketFrameReader logic
                byte[] dataWithOpcode = new byte[payload.Length + 1];
                dataWithOpcode[0] = (byte)packet.Opcode; // Opcode is a ushort, but protocol uses 1 byte. Potential truncation.
                                                         // The Packet class should ideally store opcode as byte if that's the wire protocol.
                                                         // For now, casting. This needs to be consistent with PacketFrameReader.
                Array.Copy(payload, 0, dataWithOpcode, 1, payload.Length);

                ushort dataLength = (ushort)dataWithOpcode.Length;

                byte[] header = BitConverter.GetBytes(PacketFrameReader.ExpectedHeader);
                byte[] lengthBytes = BitConverter.GetBytes(dataLength); // Little Endian
                byte[] tail = BitConverter.GetBytes(PacketFrameReader.ExpectedTail);

                // Construct the full message
                using (var ms = new MemoryStream())
                {
                    ms.Write(header, 0, header.Length);
                    ms.Write(lengthBytes, 0, lengthBytes.Length);
                    ms.Write(dataWithOpcode, 0, dataWithOpcode.Length);
                    ms.Write(tail, 0, tail.Length);

                    byte[] fullMessage = ms.ToArray();
                    await _networkStream.WriteAsync(fullMessage, 0, fullMessage.Length, _sessionCts.Token); // Use session CTS for send ops
                    await _networkStream.FlushAsync(_sessionCts.Token);
                    // _logger.LogDebug($"Session {SessionId}: Sent packet with opcode {packet.Opcode}, FullMessage Size: {fullMessage.Length}");
                    Console.WriteLine($"Session {SessionId}: Sent packet with opcode {packet.Opcode}, FullMessage Size: {fullMessage.Length}");// Placeholder
                }
            }
            catch (IOException ex)
            {
                // _logger.LogError(ex, $"Session {SessionId}: IOException during SendPacketAsync. Closing session.");
                Console.WriteLine($"Session {SessionId}: IOException during SendPacketAsync. {ex.Message}. Closing session.");// Placeholder
                Close();
            }
            catch (ObjectDisposedException ex)
            {
                // _logger.LogWarning(ex, $"Session {SessionId}: NetworkStream disposed during SendPacketAsync.");
                Console.WriteLine($"Session {SessionId}: NetworkStream disposed during SendPacketAsync. {ex.Message}.");// Placeholder
                Close(); // Ensure session is marked as closed
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, $"Session {SessionId}: Unhandled exception in SendPacketAsync. Closing session.");
                 Console.WriteLine($"Session {SessionId}: Unhandled exception in SendPacketAsync. {ex.Message}. Closing session.");// Placeholder
                Close();
            }
        }

        public void Close()
        {
            if (_tcpClient.Connected)
            {
                // _logger.LogInformation($"Session {SessionId}: Closing connection for client {_tcpClient.Client.RemoteEndPoint}.");
                Console.WriteLine($"Session {SessionId}: Closing connection for client {_tcpClient.Client.RemoteEndPoint}.");// Placeholder
            }
            _sessionCts.Cancel(); // Signal cancellation to any ongoing operations like ProcessIncomingDataAsync
            _networkStream.Close();
            _tcpClient.Close();
            _sessionCts.Dispose();
        }
    }
}
