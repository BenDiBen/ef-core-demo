using EfCoreDemo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreDemo.Persistence.Configuration;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Ignore(a => a.Transactions);
        builder.HasAlternateKey(a => a.Number);

        builder.Property(a => a.Number).HasMaxLength(10);
        builder.Property(a => a.BranchCode).HasMaxLength(3);
        builder.Property(a => a.Type).HasMaxLength(12);
    }
}