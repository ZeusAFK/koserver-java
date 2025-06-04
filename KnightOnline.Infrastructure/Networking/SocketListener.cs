using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using KnightOnline.Infrastructure.Configuration.Models; // For NetworkSettings
// using Microsoft.Extensions.Logging; // Optional: for logging

namespace KnightOnline.Infrastructure.Networking
{
    public class SocketListener
    {
        private TcpListener _listener;
        private readonly NetworkSettings _networkSettings;
        // private readonly ILogger<SocketListener> _logger; // Optional

        public bool IsListening { get; private set; }
        public IPEndPoint? LocalEndpoint => _listener?.LocalEndpoint as IPEndPoint;

        public SocketListener(NetworkSettings networkSettings /*, ILogger<SocketListener> logger = null*/)
        {
            if (networkSettings == null) throw new ArgumentNullException(nameof(networkSettings));
            // if (logger == null) _logger = new LoggerFactory().CreateLogger<SocketListener>(); // Basic default if no DI

            _networkSettings = networkSettings;
            // _logger = logger;

            // Resolve host address - TcpListener can take IPAddress directly.
            // If _networkSettings.BindHost is "*", IPAddress.Any is appropriate.
            IPAddress ipAddress = _networkSettings.BindHost == "*" || _networkSettings.BindHost.ToLower() == "any"
                ? IPAddress.Any
                : IPAddress.Parse(_networkSettings.BindHost); // This can throw if host is not a valid IP. Add DNS resolution if hostname support is needed.

            _listener = new TcpListener(ipAddress, _networkSettings.BindPort);
        }

        public void Start()
        {
            if (IsListening)
            {
                // _logger?.LogWarning("Listener is already started.");
                Console.WriteLine("Listener is already started."); // Placeholder log
                return;
            }

            try
            {
                _listener.Start();
                IsListening = true;
                // _logger?.LogInformation($"Server started listening on {LocalEndpoint}");
                Console.WriteLine($"Server started listening on {LocalEndpoint}"); // Placeholder log
            }
            catch (SocketException ex)
            {
                // _logger?.LogError(ex, $"Failed to start listener on {_networkSettings.BindHost}:{_networkSettings.BindPort}");
                Console.WriteLine($"Failed to start listener: {ex.Message}"); // Placeholder log
                throw; // Rethrow to allow calling code to handle it
            }
        }

        public async Task<TcpClient?> AcceptClientAsync(CancellationToken cancellationToken = default)
        {
            if (!IsListening)
            {
                // _logger?.LogWarning("Listener is not started. Cannot accept clients.");
                Console.WriteLine("Listener is not started. Cannot accept clients."); // Placeholder log
                return null;
                // Or throw new InvalidOperationException("Listener is not started.");
            }

            try
            {
                // TcpListener.AcceptTcpClientAsync can take a CancellationToken in some .NET versions/targets.
                // For simplicity, if it doesn't directly, use a wrapper or check token periodically if needed for long waits.
                // However, AcceptTcpClientAsync itself is cancellable by stopping the listener.
                if (cancellationToken.IsCancellationRequested) return null;

                // This will throw OperationCanceledException if listener is stopped while waiting.
                return await _listener.AcceptTcpClientAsync(cancellationToken);
            }
            catch (ObjectDisposedException) when (!IsListening)
            {
                // Listener was stopped and disposed, expected during shutdown.
                // _logger?.LogInformation("Listener was stopped, accept operation cancelled.");
                Console.WriteLine("Listener was stopped, accept operation cancelled.");
                return null;
            }
            catch (SocketException ex)
            {
                // Handle specific socket errors if necessary, e.g., if the listener is stopped.
                // _logger?.LogError(ex, "Error accepting client connection.");
                 Console.WriteLine($"Error accepting client: {ex.Message}"); // Placeholder log
                return null; // Or rethrow depending on desired error handling
            }
            catch (Exception ex) // Catch other exceptions
            {
                // _logger?.LogError(ex, "An unexpected error occurred while accepting client connection.");
                Console.WriteLine($"Unexpected error accepting client: {ex.Message}"); // Placeholder log
                return null;
            }
        }

        public void Stop()
        {
            if (!IsListening)
            {
                return;
            }

            try
            {
                IsListening = false; // Set before stopping to prevent race conditions in AcceptClientAsync
                _listener.Stop();
                // _logger?.LogInformation($"Server stopped listening on {LocalEndpoint}");
                 Console.WriteLine("Server stopped listening."); // Placeholder log
            }
            catch (SocketException ex)
            {
                // _logger?.LogError(ex, "Error stopping the listener.");
                 Console.WriteLine($"Error stopping listener: {ex.Message}"); // Placeholder log
            }
        }
    }
}
