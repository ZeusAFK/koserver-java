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
using KnightOnline.LoginServer.Features.System.Handlers;
using KnightOnline.LoginServer.Features.Authentication.Handlers;
using KnightOnline.LoginServer.Features.Security.Handlers;
using KnightOnline.LoginServer.Features.Server.Handlers; // For ServerListRequestHandler

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

                // MediatR will scan the assembly containing AccountDto (i.e., KnightOnline.Application)
                // and register all handlers including GetServerListQueryHandler.
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<KnightOnline.Application.DTOs.AccountDto>());

                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("KnightOnlineLoginDB"));
                services.AddScoped<KnightOnline.Application.Contracts.Infrastructure.IAccountRepository, KnightOnline.Infrastructure.Data.Repositories.AccountRepository>();

                // Networking Services
                services.AddSingleton<PacketFrameReader>();
                services.AddSingleton<IPacketDispatcher, PacketDispatcher>();
                services.AddSingleton<SocketListener>();

                // Register Packet Handlers
                services.AddTransient<VersionRequestHandler>();
                services.AddTransient<LoginRequestHandler>();
                services.AddTransient<EncryptionKeyExchangeHandler>();
                services.AddTransient<ServerListRequestHandler>(); // Added this handler

                Console.WriteLine("LoginServer services configured.");
            })
            .Build();

        // Register packet handlers with the dispatcher
        var packetDispatcher = host.Services.GetRequiredService<IPacketDispatcher>();

        packetDispatcher.RegisterHandler(host.Services.GetRequiredService<VersionRequestHandler>());
        packetDispatcher.RegisterHandler(host.Services.GetRequiredService<LoginRequestHandler>());
        packetDispatcher.RegisterHandler(host.Services.GetRequiredService<EncryptionKeyExchangeHandler>());
        packetDispatcher.RegisterHandler(host.Services.GetRequiredService<ServerListRequestHandler>()); // Added this handler

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
