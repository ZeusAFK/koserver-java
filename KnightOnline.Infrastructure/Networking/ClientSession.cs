using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using KnightOnline.Application.Contracts.Networking;
using KnightOnline.Domain.Networking;
using KnightOnline.Infrastructure.Security; // For Cryptor
using System.Numerics; // For BigInteger
using System.IO; // For MemoryStream
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

        private Cryptor? _cryptor; // Added for encryption
        public bool IsEncrypted { get; private set; } = false; // Added for encryption, default to false

        public string SessionId { get; }

        public ClientSession(TcpClient tcpClient, IPacketDispatcher packetDispatcher, PacketFrameReader packetFrameReader)
        {
            _tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            _networkStream = _tcpClient.GetStream();
            _packetDispatcher = packetDispatcher ?? throw new ArgumentNullException(nameof(packetDispatcher));
            _packetFrameReader = packetFrameReader ?? throw new ArgumentNullException(nameof(packetFrameReader));
            // _logger = logger;

            SessionId = Guid.NewGuid().ToString();
            _sessionCts = new CancellationTokenSource();

            Console.WriteLine($"Session {SessionId} created for client {tcpClient.Client.RemoteEndPoint}"); // Placeholder
        }

        public void ActivateEncryption(BigInteger publicKey)
        {
            _cryptor = new Cryptor(publicKey);
            IsEncrypted = true;
            // _logger?.LogInformation($"Session {SessionId} encryption activated.");
            Console.WriteLine($"Session {SessionId} encryption activated."); // Placeholder
        }

        public async Task ProcessIncomingDataAsync(CancellationToken linkedExternalToken = default)
        {
            using (var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(_sessionCts.Token, linkedExternalToken))
            {
                var cancellationToken = combinedCts.Token;
                try
                {
                    while (_tcpClient.Connected && !cancellationToken.IsCancellationRequested)
                    {
                        // Modified to pass _cryptor
                        Packet? packet = await _packetFrameReader.ReadNextPacketAsync(_networkStream, _cryptor, cancellationToken);
                        if (packet != null)
                        {
                            Console.WriteLine($"Session {SessionId} received packet with opcode {packet.Opcode}. Dispatching...");// Placeholder
                            await _packetDispatcher.DispatchPacketAsync(this, packet);
                        }
                        else if (!cancellationToken.IsCancellationRequested)
                        {
                            Console.WriteLine($"Session {SessionId}: Remote client closed connection or stream error."); // Placeholder
                            break;
                        }
                    }
                }
                catch (InvalidDataException ex)
                {
                    Console.WriteLine($"Session {SessionId}: Invalid data received. {ex.Message}. Closing session."); // Placeholder
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Session {SessionId}: IOException. {ex.Message}. Closing session."); // Placeholder
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine($"Session {SessionId}: Processing cancelled."); // Placeholder
                }
                catch (Exception ex)
                {
                     Console.WriteLine($"Session {SessionId}: Unhandled exception. {ex.Message}. Closing session."); // Placeholder
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
                Console.WriteLine($"Session {SessionId}: Cannot send packet. Client not connected."); // Placeholder
                return;
            }

            try
            {
                byte[] payload = packet.GetPayload();
                byte[] dataWithOpcode = new byte[payload.Length + 1];
                dataWithOpcode[0] = (byte)packet.Opcode;
                Array.Copy(payload, 0, dataWithOpcode, 1, payload.Length);

                byte[] dataToSend = dataWithOpcode;
                if (IsEncrypted && _cryptor != null)
                {
                    // _logger?.LogDebug($"Session {SessionId}: Encrypting packet with opcode {packet.Opcode}");
                    Console.WriteLine($"Session {SessionId}: Encrypting packet with opcode {packet.Opcode}"); // Placeholder
                    dataToSend = _cryptor.Process(dataWithOpcode);
                }

                ushort dataLength = (ushort)dataToSend.Length;

                byte[] headerBytes = BitConverter.GetBytes(PacketFrameReader.ExpectedHeader);
                byte[] lengthBytes = BitConverter.GetBytes(dataLength);
                byte[] tailBytes = BitConverter.GetBytes(PacketFrameReader.ExpectedTail);

                using (var ms = new MemoryStream())
                {
                    ms.Write(headerBytes, 0, headerBytes.Length);
                    ms.Write(lengthBytes, 0, lengthBytes.Length);
                    ms.Write(dataToSend, 0, dataToSend.Length);
                    ms.Write(tailBytes, 0, tailBytes.Length);

                    byte[] fullMessage = ms.ToArray();
                    await _networkStream.WriteAsync(fullMessage, 0, fullMessage.Length, _sessionCts.Token);
                    await _networkStream.FlushAsync(_sessionCts.Token); // Good practice to flush
                    Console.WriteLine($"Session {SessionId}: Sent packet with opcode {packet.Opcode}, FullMessage Size: {fullMessage.Length}");// Placeholder
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Session {SessionId}: IOException during SendPacketAsync. {ex.Message}. Closing session.");// Placeholder
                Close();
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"Session {SessionId}: NetworkStream disposed during SendPacketAsync. {ex.Message}.");// Placeholder
                Close();
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"Session {SessionId}: Unhandled exception in SendPacketAsync. {ex.Message}. Closing session.");// Placeholder
                Close();
            }
        }

        public void Close()
        {
            if (_tcpClient.Connected)
            {
                Console.WriteLine($"Session {SessionId}: Closing connection for client {_tcpClient.Client.RemoteEndPoint}.");// Placeholder
            }
            // Ensure cancellation is requested only once and then resources are cleaned up.
            if (!_sessionCts.IsCancellationRequested)
            {
                _sessionCts.Cancel();
            }

            // It's safe to call Close on streams multiple times.
            _networkStream.Close();
            _tcpClient.Close();

            // Dispose CancellationTokenSource only when it's guaranteed no longer in use.
            // If ProcessIncomingDataAsync or SendPacketAsync could still be running and referencing _sessionCts.Token,
            // disposing it here might be premature. However, after streams are closed, they should exit.
            // For simplicity in this context, let's assume this is fine.
            _sessionCts.Dispose();
        }
    }
}
