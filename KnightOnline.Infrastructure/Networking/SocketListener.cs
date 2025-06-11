using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using KnightOnline.Application.Contracts.Networking;
using KnightOnline.Infrastructure.Configuration.Models;
// using Microsoft.Extensions.Logging; // Optional: If using ILogger
using Microsoft.Extensions.Hosting; // Required for IHostedService

namespace KnightOnline.Infrastructure.Networking
{
    public class SocketListener : IHostedService // Implement IHostedService
    {
        private TcpListener? _listener; // Nullable if not created in constructor
        private readonly NetworkSettings _networkSettings;
        private readonly IPacketDispatcher _packetDispatcher;
        private readonly PacketFrameReader _packetFrameReader;
        // private readonly ILogger<SocketListener> _logger; // Optional

        private readonly List<IClientSession> _activeSessions = new List<IClientSession>();
        private CancellationTokenSource _serverCts = new CancellationTokenSource();

        public bool IsListening { get; private set; }
        public IPEndPoint? LocalEndpoint => _listener?.LocalEndpoint as IPEndPoint;

        public SocketListener(NetworkSettings networkSettings, IPacketDispatcher packetDispatcher, PacketFrameReader packetFrameReader /*, ILogger<SocketListener> logger = null */)
        {
            _networkSettings = networkSettings ?? throw new ArgumentNullException(nameof(networkSettings));
            _packetDispatcher = packetDispatcher ?? throw new ArgumentNullException(nameof(packetDispatcher));
            _packetFrameReader = packetFrameReader ?? throw new ArgumentNullException(nameof(packetFrameReader));
            // _logger = logger;
            // Listener is initialized in StartAsync now to handle potential startup failures better with IHostedService
        }

        public Task StartAsync(CancellationToken cancellationToken) // IHostedService method
        {
            // _logger?.LogInformation("SocketListener starting...");
            Console.WriteLine("SocketListener starting..."); // Placeholder

            // Link the CancellationTokenSource with the one provided by the Host
            _serverCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            IPAddress ipAddress = _networkSettings.BindHost == "*" || _networkSettings.BindHost.ToLower() == "any"
                ? IPAddress.Any
                : IPAddress.Parse(_networkSettings.BindHost);
            _listener = new TcpListener(ipAddress, _networkSettings.BindPort);

            if (IsListening)
            {
                // _logger?.LogWarning("Listener is already started.");
                Console.WriteLine("Listener is already started."); // Placeholder
                return Task.CompletedTask;
            }

            try
            {
                _listener.Start();
                IsListening = true;
                // _logger?.LogInformation($"Server started listening on {LocalEndpoint}. Starting accept loop...");
                Console.WriteLine($"Server started listening on {LocalEndpoint}. Starting accept loop..."); // Placeholder

                // Start the accept loop
                _ = Task.Run(() => AcceptLoopAsync(_serverCts.Token), _serverCts.Token); // Fire and forget, but track via _serverCts
            }
            catch (SocketException ex)
            {
                // _logger?.LogError(ex, $"Failed to start listener on {_networkSettings.BindHost}:{_networkSettings.BindPort}");
                Console.WriteLine($"Failed to start listener: {ex.Message}"); // Placeholder
                IsListening = false;
                // Propagate the exception to inform the host that the service failed to start
                return Task.FromException(ex);
            }
            return Task.CompletedTask;
        }

        private async Task AcceptLoopAsync(CancellationToken cancellationToken)
        {
            // _logger?.LogInformation("Accept loop started.");
            Console.WriteLine("Accept loop started."); // Placeholder
            try
            {
                while (!cancellationToken.IsCancellationRequested && _listener != null)
                {
                    TcpClient acceptedTcpClient = await _listener.AcceptTcpClientAsync(cancellationToken);
                    // _logger?.LogInformation($"Accepted new client from: {acceptedTcpClient.Client.RemoteEndPoint}");
                    Console.WriteLine($"Accepted new client from: {acceptedTcpClient.Client.RemoteEndPoint}");// Placeholder

                    var clientSession = new ClientSession(acceptedTcpClient, _packetDispatcher, _packetFrameReader /*, _loggerFactory.CreateLogger<ClientSession>() */);

                    lock (_activeSessions)
                    {
                        _activeSessions.Add(clientSession);
                    }

                    _ = Task.Run(() => clientSession.ProcessIncomingDataAsync(cancellationToken)
                        .ContinueWith(t => {
                            // _logger?.LogInformation($"Session {clientSession.SessionId} processing ended.");
                            Console.WriteLine($"Session {clientSession.SessionId} processing ended.");// Placeholder
                            lock(_activeSessions)
                            {
                                _activeSessions.Remove(clientSession);
                            }
                            clientSession.Close();
                        }, TaskScheduler.Default), cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // _logger?.LogInformation("Accept loop was cancelled (server shutting down).");
                Console.WriteLine("Accept loop was cancelled (server shutting down).");// Placeholder
            }
            catch (Exception ex)
            {
                // _logger?.LogError(ex, "Exception in accept loop.");
                 Console.WriteLine($"Exception in accept loop: {ex.Message}");// Placeholder
            }
            finally
            {
                IsListening = false;
                // _logger?.LogInformation("Accept loop finished.");
                Console.WriteLine("Accept loop finished.");// Placeholder
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) // IHostedService method
        {
            // _logger?.LogInformation("SocketListener stopping...");
            Console.WriteLine("SocketListener stopping..."); // Placeholder

            if (!IsListening && (_serverCts == null || _serverCts.IsCancellationRequested))
            {
                // _logger?.LogInformation("Listener is already stopped or stopping.");
                Console.WriteLine("Listener is already stopped or stopping.");// Placeholder
                return Task.CompletedTask;
            }

            _serverCts?.Cancel();

            if (IsListening && _listener != null)
            {
                 _listener.Stop();
            }
            IsListening = false;

            // _logger?.LogInformation("Server listener commanded to stop. Waiting for sessions to close...");
            Console.WriteLine("Server listener commanded to stop. Waiting for sessions to close...");// Placeholder

            // List<Task> sessionCloseTasks = new List<Task>(); // Not used currently
            lock (_activeSessions)
            {
                foreach (var session in _activeSessions.ToList())
                {
                    session.Close();
                }
                _activeSessions.Clear();
            }

            // _logger?.LogInformation("SocketListener stopped.");
            Console.WriteLine("SocketListener stopped.");// Placeholder
            _serverCts?.Dispose();
            return Task.CompletedTask;
        }
    }
}
