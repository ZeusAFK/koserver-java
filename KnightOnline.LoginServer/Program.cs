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
using KnightOnline.LoginServer.Features.Authentication.Handlers; // For LoginRequestHandler

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
                // and register all handlers including LoginCommandHandler.
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<KnightOnline.Application.DTOs.AccountDto>());

                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("KnightOnlineLoginDB"));
                services.AddScoped<KnightOnline.Application.Contracts.Infrastructure.IAccountRepository, KnightOnline.Infrastructure.Data.Repositories.AccountRepository>();

                // Networking Services
                services.AddSingleton<PacketFrameReader>();
                services.AddSingleton<IPacketDispatcher, PacketDispatcher>();
                services.AddSingleton<SocketListener>();

                // Register Packet Handlers
                // VersionRequestHandler depends on AppSettings (Singleton), can be Transient or Singleton.
                services.AddTransient<VersionRequestHandler>();
                // LoginRequestHandler depends on IMediator (Scoped from AddMediatR, effectively), can be Transient.
                services.AddTransient<LoginRequestHandler>();

                Console.WriteLine("LoginServer services configured.");
            })
            .Build();

        // Register packet handlers with the dispatcher
        var packetDispatcher = host.Services.GetRequiredService<IPacketDispatcher>();

        // Resolve and register VersionRequestHandler
        var versionHandler = host.Services.GetRequiredService<VersionRequestHandler>();
        packetDispatcher.RegisterHandler(versionHandler);

        // Resolve and register LoginRequestHandler
        var loginHandler = host.Services.GetRequiredService<LoginRequestHandler>();
        packetDispatcher.RegisterHandler(loginHandler);

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
