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
using KnightOnline.LoginServer.Features.System.Handlers; // For VersionCheckHandler
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

            ConfigurePacketHandlers(host.Services);

            Console.WriteLine("LoginServer starting host.RunAsync()...");
            await host.RunAsync();
            Console.WriteLine("LoginServer host has stopped.");
        }

        static void ConfigurePacketHandlers(IServiceProvider services)
        {
            var dispatcher = services.GetRequiredService<IPacketDispatcher>();

            // Resolve and register all required handlers
            dispatcher.RegisterHandler(services.GetRequiredService<VersionCheckHandler>());
            dispatcher.RegisterHandler(services.GetRequiredService<LoginRequestHandler>());
            dispatcher.RegisterHandler(services.GetRequiredService<EncryptionKeyExchangeHandler>());
            dispatcher.RegisterHandler(services.GetRequiredService<ServerListRequestHandler>());
            dispatcher.RegisterHandler(services.GetRequiredService<NewsRequestHandler>());

            Console.WriteLine("Packet handlers registered with dispatcher.");
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
                    services.AddScoped<KnightOnline.Application.Contracts.Infrastructure.IServerDetailRepository, KnightOnline.Infrastructure.Data.Repositories.ServerDetailRepository>();

                    // Networking Services
                    services.AddSingleton<PacketFrameReader>();
                    services.AddSingleton<IPacketDispatcher, PacketDispatcher>();

                    // Register SocketListener as IHostedService
                    services.AddSingleton<IHostedService, SocketListener>(serviceProvider =>
                    {
                        var loginServerSettings = serviceProvider.GetRequiredService<IOptions<LoginServerSettings>>().Value;
                        if (loginServerSettings?.Network == null)
                        {
                            throw new InvalidOperationException("NetworkSettings is not configured within LoginServerSettings.");
                        }

                        var packetDispatcher = serviceProvider.GetRequiredService<IPacketDispatcher>();
                        var packetFrameReader = serviceProvider.GetRequiredService<PacketFrameReader>();
                        // var logger = serviceProvider.GetService<ILogger<SocketListener>>();

                        return new SocketListener(loginServerSettings.Network, packetDispatcher, packetFrameReader /*, logger */);
                    });

                    // Register Packet Handlers as transient services
                    services.AddTransient<VersionCheckHandler>(); // Changed from VersionRequestHandler
                    services.AddTransient<LoginRequestHandler>();
                    services.AddTransient<EncryptionKeyExchangeHandler>();
                    services.AddTransient<ServerListRequestHandler>();
                    services.AddTransient<NewsRequestHandler>();

                    Console.WriteLine("LoginServer services configured.");
                });
    }
}
