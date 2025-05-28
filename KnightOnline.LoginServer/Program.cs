using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
// using KnightOnline.Application; // For MediatR registration from Application layer - MediatR now added directly
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
                // For now, let's do it more explicitly if AssemblyReference isn't created.
                // Using a type from KnightOnline.Application to get its assembly for MediatR registration.
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<KnightOnline.Application.DTOs.AccountDto>());


                // Register EF Core DbContext
                // Using InMemory for now, connection string would come from IConfiguration in a real app
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("KnightOnlineLoginDB"));

                // Register Repositories (example)
                services.AddScoped<IAccountRepository, AccountRepository>();
                // services.AddScoped<IPlayerRepository, PlayerRepository>(); // Not typically needed by LoginServer directly

                // Register other services from Application or Infrastructure as needed
                // e.g., services.AddTransient<INetworkService, NetworkService>();

                // TODO: Add other services for LoginServer
                Console.WriteLine("LoginServer services configured.");
            })
            .Build();

        Console.WriteLine("LoginServer starting...");
        // Application logic would start here, e.g.
        // var networkService = host.Services.GetRequiredService<INetworkService>();
        // await networkService.StartServerAsync(15100);

        await host.RunAsync(); // Or custom run logic
        Console.WriteLine("LoginServer stopped.");
    }
}
