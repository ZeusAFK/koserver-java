using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KnightOnline.Domain.Accounts;

namespace KnightOnline.Infrastructure.Data.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Id);

            // Configure Value Object ID for EF Core
            // This tells EF Core how to map the UserId Value Object to a database column.
            builder.Property(a => a.Id)
                   .HasConversion(userId => userId.Value, // To Guid in database
                                  dbValue => UserId.FromGuid(dbValue)) // From Guid in database
                   .ValueGeneratedNever(); // Or ValueGeneratedOnAdd() if appropriate

            builder.Property(a => a.Username)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(a => a.Username).IsUnique();

            builder.Property(a => a.Email)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.HasIndex(a => a.Email).IsUnique();

            // Other properties...
            // Relationships...
        }
    }
}
