using Microsoft.EntityFrameworkCore;
using KnightOnline.Domain.Accounts; // For Account entity
using KnightOnline.Domain.Players;   // For Player entity
using KnightOnline.Domain.Game.Items; // For Item, ItemDefinition entities
using System.Reflection;

namespace KnightOnline.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet properties for your Aggregate Roots
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemDefinition> ItemDefinitions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply all configurations from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Example of manual configuration if not using IEntityTypeConfiguration classes:
            // modelBuilder.Entity<Account>(entity =>
            // {
            //     entity.HasKey(a => a.Id);
            //     entity.Property(a => a.Id).HasConversion(
            //         userId => userId.Value, // To Guid
            //         value => UserId.FromGuid(value) // From Guid
            //     );
            //     entity.Property(a => a.Username).IsRequired().HasMaxLength(50);
            //     // ... other configurations
            // });
        }
    }
}
