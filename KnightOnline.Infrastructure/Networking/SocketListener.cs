using System;
using System.Collections.Generic; // For List
using System.Linq; // For List manipulation
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using KnightOnline.Application.Contracts.Networking; // For IClientSession, IPacketDispatcher
using KnightOnline.Infrastructure.Configuration.Models; // For NetworkSettings
// using Microsoft.Extensions.Logging; // Optional
// using Microsoft.Extensions.DependencyInjection; // For IServiceProvider if resolving dispatcher/reader

namespace KnightOnline.Infrastructure.Networking
{
    public class SocketListener // Consider making this IDisposable if it manages many resources
    {
        private TcpListener _listener;
        private readonly NetworkSettings _networkSettings;
        // private readonly ILogger<SocketListener> _logger; // Optional
        private readonly IPacketDispatcher _packetDispatcher; // To pass to ClientSession
        private readonly PacketFrameReader _packetFrameReader; // To pass to ClientSession
        // private readonly IServiceProvider _serviceProvider; // To resolve IClientSession instances or their dependencies

        private readonly List<IClientSession> _activeSessions = new List<IClientSession>();
        private readonly CancellationTokenSource _serverCts = new CancellationTokenSource(); // For shutting down the server accept loop

        public bool IsListening { get; private set; }
        public IPEndPoint? LocalEndpoint => _listener?.LocalEndpoint as IPEndPoint;

        // Constructor updated to receive dependencies for ClientSession
        // public SocketListener(NetworkSettings networkSettings, IPacketDispatcher packetDispatcher, PacketFrameReader packetFrameReader, IServiceProvider serviceProvider, ILogger<SocketListener> logger)
        public SocketListener(NetworkSettings networkSettings, IPacketDispatcher packetDispatcher, PacketFrameReader packetFrameReader)
        {
            _networkSettings = networkSettings ?? throw new ArgumentNullException(nameof(networkSettings));
            _packetDispatcher = packetDispatcher ?? throw new ArgumentNullException(nameof(packetDispatcher));
            _packetFrameReader = packetFrameReader ?? throw new ArgumentNullException(nameof(packetFrameReader));
            // _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            // _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            IPAddress ipAddress = _networkSettings.BindHost == "*" || _networkSettings.BindHost.ToLower() == "any"
                ? IPAddress.Any
                : IPAddress.Parse(_networkSettings.BindHost);

            _listener = new TcpListener(ipAddress, _networkSettings.BindPort);
        }

        public void Start() // This method can remain synchronous for starting the listener itself
        {
            if (IsListening)
            {
                // _logger.LogWarning("Listener is already started.");
                Console.WriteLine("Listener is already started."); // Placeholder
                return;
            }

            try
            {
                _listener.Start();
                IsListening = true;
                // _logger.LogInformation($"Server started listening on {LocalEndpoint}. Starting accept loop...");
                Console.WriteLine($"Server started listening on {LocalEndpoint}. Starting accept loop..."); // Placeholder

                // Start the accept loop on a new task so Start() can return
                Task.Run(() => AcceptLoopAsync(_serverCts.Token), _serverCts.Token);
            }
            catch (SocketException ex)
            {
                // _logger.LogError(ex, $"Failed to start listener on {_networkSettings.BindHost}:{_networkSettings.BindPort}");
                 Console.WriteLine($"Failed to start listener: {ex.Message}"); // Placeholder
                IsListening = false; // Ensure state is correct
                throw;
            }
        }

        private async Task AcceptLoopAsync(CancellationToken cancellationToken)
        {
            // _logger.LogInformation("Accept loop started.");
            Console.WriteLine("Accept loop started."); // Placeholder
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    TcpClient acceptedTcpClient = await _listener.AcceptTcpClientAsync(cancellationToken);
                    // _logger.LogInformation($"Accepted new client from: {acceptedTcpClient.Client.RemoteEndPoint}");
                    Console.WriteLine($"Accepted new client from: {acceptedTcpClient.Client.RemoteEndPoint}");// Placeholder

                    // Create and start a new session.
                    // In a DI scenario, you might resolve IClientSession or its factory.
                    // var clientSession = _serviceProvider.GetRequiredService<IClientSession>(acceptedTcpClient); // This is not how GetRequiredService works with params
                    // For now, direct instantiation:
                    var clientSession = new ClientSession(acceptedTcpClient, _packetDispatcher, _packetFrameReader /*, _loggerFactory.CreateLogger<ClientSession>() */);

                    // Basic session tracking (needs thread safety for production)
                    // Consider ConcurrentDictionary<string, IClientSession> sessions;
                    lock (_activeSessions) // Basic synchronization
                    {
                        _activeSessions.Add(clientSession);
                    }

                    // Start processing data for this session on a new task
                    // Pass the server's cancellation token so session processing stops if server stops
                    _ = Task.Run(() => clientSession.ProcessIncomingDataAsync(cancellationToken)
                        .ContinueWith(t => {
                            // _logger.LogInformation($"Session {clientSession.SessionId} processing ended.");
                            Console.WriteLine($"Session {clientSession.SessionId} processing ended.");// Placeholder
                            lock(_activeSessions) // Basic synchronization
                            {
                                _activeSessions.Remove(clientSession);
                            }
                            clientSession.Close(); // Ensure closed
                        }, TaskScheduler.Default), cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // _logger.LogInformation("Accept loop was cancelled (server shutting down).");
                Console.WriteLine("Accept loop was cancelled (server shutting down).");// Placeholder
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Exception in accept loop.");
                Console.WriteLine($"Exception in accept loop: {ex.Message}");// Placeholder
            }
            finally
            {
                IsListening = false;
                // _logger.LogInformation("Accept loop finished.");
                Console.WriteLine("Accept loop finished.");// Placeholder
            }
        }

        // AcceptClientAsync is no longer needed as a public method; the loop handles it.
        // public async Task<TcpClient?> AcceptClientAsync(CancellationToken cancellationToken = default) ...

        public void Stop()
        {
            if (!IsListening && _serverCts.IsCancellationRequested)
            {
                // _logger.LogInformation("Listener is already stopped or stopping.");
                Console.WriteLine("Listener is already stopped or stopping.");// Placeholder
                return;
            }

            // _logger.LogInformation("Stopping server listener...");
            Console.WriteLine("Stopping server listener...");// Placeholder

            _serverCts.Cancel(); // Signal the accept loop and session processing to stop

            // _listener.Stop() will cause AcceptTcpClientAsync to throw an exception,
            // which is caught by the accept loop.
            // It's important that _serverCts.Cancel() is called first to signal graceful shutdown.
            if (IsListening) // Check if it was actually listening
            {
                 _listener.Stop(); // This makes AcceptTcpClientAsync throw
            }
            IsListening = false; // Set state

            // _logger.LogInformation("Server listener commanded to stop. Waiting for sessions to close...");
            Console.WriteLine("Server listener commanded to stop. Waiting for sessions to close...");// Placeholder

            // Basic wait for active sessions to clear (not robust, for illustration)
            // In production, more sophisticated shutdown logic for sessions would be needed.
            // Consider Task.WhenAll for all session processing tasks if you track them.
            lock (_activeSessions)
            {
                foreach (var session in _activeSessions.ToList()) // ToList to avoid modification issues during iteration
                {
                    session.Close();
                }
                _activeSessions.Clear();
            }
            // _logger.LogInformation("All active sessions closed. Listener stopped.");
            Console.WriteLine("All active sessions closed. Listener stopped.");// Placeholder
            _serverCts.Dispose();
        }
    }
}
