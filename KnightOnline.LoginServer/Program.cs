using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using KnightOnline.Application.Contracts.Networking;
using KnightOnline.Infrastructure.Networking;
using KnightOnline.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using KnightOnline.Infrastructure.Configuration; // For AppSettings
using KnightOnline.Infrastructure.Configuration.Models; // For LoginServerSettings
using System;
using System.Threading.Tasks;
using KnightOnline.LoginServer.Features.System.Handlers;
using KnightOnline.LoginServer.Features.Authentication.Handlers;
using KnightOnline.LoginServer.Features.Security.Handlers;
using KnightOnline.LoginServer.Features.Server.Handlers;
using KnightOnline.LoginServer.Features.News.Handlers; // For NewsRequestHandler

namespace KnightOnline.LoginServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Register packet handlers with the dispatcher
            var packetDispatcher = host.Services.GetRequiredService<IPacketDispatcher>();

            packetDispatcher.RegisterHandler(host.Services.GetRequiredService<VersionRequestHandler>());
            packetDispatcher.RegisterHandler(host.Services.GetRequiredService<LoginRequestHandler>());
            packetDispatcher.RegisterHandler(host.Services.GetRequiredService<EncryptionKeyExchangeHandler>());
            packetDispatcher.RegisterHandler(host.Services.GetRequiredService<ServerListRequestHandler>());
            packetDispatcher.RegisterHandler(host.Services.GetRequiredService<NewsRequestHandler>());

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
            // For testing configuration, you could resolve and print some settings:
            /*
            var loginSettings = host.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<LoginServerSettings>>().Value;
            Console.WriteLine($"Login Server Port: {loginSettings.Network.BindPort}");
            var appSettings = host.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<AppSettings>>().Value;
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
                    // Register the root AppSettings
                    services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));

                    // Register specific settings classes for more direct injection
                    services.Configure<LoginServerSettings>(hostContext.Configuration.GetSection("AppSettings:LoginServer"));

                    // MediatR will scan the assembly containing AccountDto (i.e., KnightOnline.Application)
                    // and register all handlers including GetNewsQueryHandler.
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
                    services.AddTransient<ServerListRequestHandler>();
                    services.AddTransient<NewsRequestHandler>();

                    Console.WriteLine("LoginServer services configured.");
                });
    }
}
