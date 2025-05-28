using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
// using KnightOnline.Application; // For MediatR registration - MediatR now added directly
using KnightOnline.Infrastructure.Data; // For AppDbContext
using Microsoft.EntityFrameworkCore; // For UseInMemoryDatabase
using KnightOnline.Application.Contracts.Infrastructure; // For repository interfaces
using KnightOnline.Infrastructure.Data.Repositories; // For repository implementations
using System; // For Console
using System.Threading.Tasks; // For Task

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Register MediatR
                // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(KnightOnline.Application.AssemblyReference).Assembly));
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<KnightOnline.Application.DTOs.AccountDto>());

                // Register EF Core DbContext
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("KnightOnlineGameDB")); // Separate DB for game server

                // Register Repositories (example)
                services.AddScoped<IAccountRepository, AccountRepository>(); // May be needed for account validation on game server connect
                services.AddScoped<IPlayerRepository, PlayerRepository>();

                // TODO: Add other services for GameServer
                Console.WriteLine("GameServer services configured.");
            })
            .Build();

        Console.WriteLine("GameServer starting...");
        // Application logic would start here

        await host.RunAsync(); // Or custom run logic
        Console.WriteLine("GameServer stopped.");
    }
}
