using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using KnightOnline.Application.Contracts.Networking;
using KnightOnline.Infrastructure.Networking;
using KnightOnline.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using KnightOnline.Infrastructure.Configuration;
using KnightOnline.Infrastructure.Configuration.Models;
using System;
using System.Threading.Tasks;
using KnightOnline.LoginServer.Features.System.Handlers; // For VersionRequestHandler

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                // Configuration setup
            })
            .ConfigureServices((hostContext, services) =>
            {
                var appSettings = ConfigLoader.Load();
                services.AddSingleton(appSettings);
                services.AddSingleton(appSettings.LoginServer.Network);

                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<KnightOnline.Application.DTOs.AccountDto>());
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("KnightOnlineLoginDB"));
                services.AddScoped<KnightOnline.Application.Contracts.Infrastructure.IAccountRepository, KnightOnline.Infrastructure.Data.Repositories.AccountRepository>();

                // Networking Services
                services.AddSingleton<PacketFrameReader>();
                services.AddSingleton<IPacketDispatcher, PacketDispatcher>();
                services.AddSingleton<SocketListener>();

                // Register Packet Handlers (as Scoped or Transient if they have scoped dependencies like DbContext)
                // For VersionRequestHandler, it depends on AppSettings (Singleton), so it can be Singleton or Transient.
                // Let's register it as Transient for now, which is a safe default for handlers.
                services.AddTransient<VersionRequestHandler>();
                // If there were many handlers, you might use assembly scanning to register all IPacketHandler implementations.

                Console.WriteLine("LoginServer services configured.");
            })
            .Build();

        // Register packet handlers with the dispatcher
        var packetDispatcher = host.Services.GetRequiredService<IPacketDispatcher>();

        // Resolve and register VersionRequestHandler
        // If handlers are registered as services themselves (as above with AddTransient)
        var versionHandler = host.Services.GetRequiredService<VersionRequestHandler>();
        packetDispatcher.RegisterHandler(versionHandler);
        // Alternatively, if handlers were not in DI but simple to instantiate:
        // packetDispatcher.RegisterHandler(new VersionRequestHandler(host.Services.GetRequiredService<AppSettings>()));

        Console.WriteLine("Packet handlers registered.");

        // Start the SocketListener
        var listener = host.Services.GetRequiredService<SocketListener>();
        listener.Start();

        // Setup graceful shutdown
        var applicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
        applicationLifetime.ApplicationStopping.Register(() =>
        {
            Console.WriteLine("LoginServer application stopping. Stopping listener...");
            listener.Stop();
        });

        Console.WriteLine("LoginServer starting host.RunAsync()...");
        await host.RunAsync();
        Console.WriteLine("LoginServer host has stopped.");
    }
}
