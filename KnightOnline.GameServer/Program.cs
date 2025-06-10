using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using KnightOnline.Application.Contracts.Networking;
using KnightOnline.Infrastructure.Networking;
using KnightOnline.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using KnightOnline.Infrastructure.Configuration;
using KnightOnline.Infrastructure.Configuration.Models;
using System; // For Console
using System.Threading.Tasks; // For Task

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
                services.AddSingleton(appSettings.GameServer.Network);

                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<KnightOnline.Application.DTOs.AccountDto>());
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("KnightOnlineGameDB"));
                services.AddScoped<KnightOnline.Application.Contracts.Infrastructure.IAccountRepository, KnightOnline.Infrastructure.Data.Repositories.AccountRepository>();
                services.AddScoped<KnightOnline.Application.Contracts.Infrastructure.IPlayerRepository, KnightOnline.Infrastructure.Data.Repositories.PlayerRepository>();

                services.AddSingleton<PacketFrameReader>();
                services.AddSingleton<IPacketDispatcher, PacketDispatcher>();
                services.AddSingleton<SocketListener>();

                Console.WriteLine("GameServer services configured.");
            })
            .Build();

        // Start the SocketListener
        var listener = host.Services.GetRequiredService<SocketListener>();
        listener.Start(); // Start listening for connections

        // Setup graceful shutdown for the listener
        var applicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
        applicationLifetime.ApplicationStopping.Register(() =>
        {
            Console.WriteLine("GameServer application stopping. Stopping listener...");
            listener.Stop();
        });

        Console.WriteLine("GameServer starting host.RunAsync()...");
        await host.RunAsync();
        Console.WriteLine("GameServer host has stopped.");
    }
}
