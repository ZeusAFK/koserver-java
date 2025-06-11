using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options; // Required for IOptions
// using Microsoft.Extensions.Logging; // Optional, if ILogger is to be used
using KnightOnline.Application.Contracts.Networking;
using KnightOnline.Infrastructure.Networking;
using KnightOnline.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using KnightOnline.Infrastructure.Configuration; // For AppSettings
using KnightOnline.Infrastructure.Configuration.Models; // For LoginServerSettings, NetworkSettings
using System;
using System.Threading.Tasks;
using KnightOnline.LoginServer.Features.System.Handlers;
using KnightOnline.LoginServer.Features.Authentication.Handlers;
using KnightOnline.LoginServer.Features.Security.Handlers;
using KnightOnline.LoginServer.Features.Server.Handlers;
using KnightOnline.LoginServer.Features.News.Handlers;

namespace KnightOnline.LoginServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Manual packet handler registration and listener start is removed.
            // IHostedService will manage SocketListener.
            // Packet handler registration should be done when PacketDispatcher is built,
            // or via another mechanism if handlers are resolved from DI.

            Console.WriteLine("LoginServer starting host.RunAsync()...");
            // For testing configuration, you could resolve and print some settings:
            /*
            var loginSettings = host.Services.GetRequiredService<IOptions<LoginServerSettings>>().Value;
            Console.WriteLine($"Login Server Port: {loginSettings.Network.BindPort}");
            var appSettings = host.Services.GetRequiredService<IOptions<AppSettings>>().Value;
            Console.WriteLine($"Login Server FTP URL from AppSettings: {appSettings.LoginServer.FtpUrl}");
            */
            await host.RunAsync();
            Console.WriteLine("LoginServer host has stopped.");
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
                    services.Configure<LoginServerSettings>(hostContext.Configuration.GetSection("AppSettings:LoginServer"));

                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<KnightOnline.Application.DTOs.AccountDto>());

                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase("KnightOnlineLoginDB"));
                    services.AddScoped<KnightOnline.Application.Contracts.Infrastructure.IAccountRepository, KnightOnline.Infrastructure.Data.Repositories.AccountRepository>();
                    // Register IServerDetailRepository if it's used by any services that might be resolved
                    services.AddScoped<KnightOnline.Application.Contracts.Infrastructure.IServerDetailRepository, KnightOnline.Infrastructure.Data.Repositories.ServerDetailRepository>();


                    // Networking Services
                    services.AddSingleton<PacketFrameReader>();
                    services.AddSingleton<IPacketDispatcher, PacketDispatcher>();

                    // Register SocketListener as IHostedService
                    services.AddSingleton<IHostedService, SocketListener>(serviceProvider =>
                    {
                        // Resolve NetworkSettings from IOptions<LoginServerSettings>
                        var loginServerSettings = serviceProvider.GetRequiredService<IOptions<LoginServerSettings>>().Value;
                        if (loginServerSettings?.Network == null)
                        {
                            throw new InvalidOperationException("NetworkSettings is not configured within LoginServerSettings.");
                        }

                        var packetDispatcher = serviceProvider.GetRequiredService<IPacketDispatcher>();
                        var packetFrameReader = serviceProvider.GetRequiredService<PacketFrameReader>();
                        // var logger = serviceProvider.GetService<ILogger<SocketListener>>(); // Optional: if you want to inject logger

                        // TODO: The IPacketDispatcher needs to be populated with its handlers.
                        // This could be done here by resolving all IMessageHandler implementations and registering them,
                        // or PacketDispatcher could take IEnumerable<IMessageHandler> in its constructor,
                        // and DI would provide them if they are registered.
                        // For now, dispatcher is "empty" as per prompt.
                        // Example of manual registration if needed (but better to do it via DI if handlers are services):
                        /*
                        var versionHandler = serviceProvider.GetRequiredService<VersionRequestHandler>();
                        packetDispatcher.RegisterHandler(versionHandler);
                        // ... register other handlers ...
                        */

                        return new SocketListener(loginServerSettings.Network, packetDispatcher, packetFrameReader /*, logger */);
                    });

                    // Register Packet Handlers (if they are to be resolved by PacketDispatcher itself, or for other uses)
                    // If PacketDispatcher is responsible for resolving its own handlers, this might not be needed here.
                    // If they are injected into PacketDispatcher, then PacketDispatcher needs to be modified.
                    // For now, keeping them as transient services if they need to be resolved by something.
                    services.AddTransient<VersionRequestHandler>();
                    services.AddTransient<LoginRequestHandler>();
                    services.AddTransient<EncryptionKeyExchangeHandler>();
                    services.AddTransient<ServerListRequestHandler>();
                    services.AddTransient<NewsRequestHandler>();

                    Console.WriteLine("LoginServer services configured.");
                });
    }
}
