using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using KnightOnline.Application.Contracts.Networking;
using KnightOnline.Infrastructure.Networking;
using KnightOnline.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using KnightOnline.Infrastructure.Configuration; // For AppSettings
using KnightOnline.Infrastructure.Configuration.Models; // For GameServerSettings
using System; // For Console
using System.Threading.Tasks; // For Task

namespace KnightOnline.GameServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

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

            Console.WriteLine("Game Server Host Built. (Actual server logic not started in this placeholder)");
            // Example of resolving and printing settings:
            /*
            var gameSettings = host.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<GameServerSettings>>().Value;
            Console.WriteLine($"Game Server Port: {gameSettings.Network.BindPort}");
            var appSettings = host.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<AppSettings>>().Value;
            Console.WriteLine($"Game Server Version from AppSettings: {appSettings.GameServer.Version}");
            */
            Console.WriteLine("GameServer starting host.RunAsync()...");
            await host.RunAsync();
            Console.WriteLine("GameServer host has stopped.");
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppContext.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));
                    services.Configure<GameServerSettings>(hostContext.Configuration.GetSection("AppSettings:GameServer"));

                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<KnightOnline.Application.DTOs.AccountDto>());
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase("KnightOnlineGameDB"));
                    services.AddScoped<KnightOnline.Application.Contracts.Infrastructure.IAccountRepository, KnightOnline.Infrastructure.Data.Repositories.AccountRepository>();
                    services.AddScoped<KnightOnline.Application.Contracts.Infrastructure.IPlayerRepository, KnightOnline.Infrastructure.Data.Repositories.PlayerRepository>();

                    services.AddSingleton<PacketFrameReader>();
                    services.AddSingleton<IPacketDispatcher, PacketDispatcher>();
                    services.AddSingleton<SocketListener>();

                    Console.WriteLine("GameServer services configured.");
                });
    }
}
