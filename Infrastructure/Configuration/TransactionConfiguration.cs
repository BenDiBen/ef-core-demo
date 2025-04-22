using EfCoreDemo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreDemo.Infrastructure.Configuration;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.HasOne<Account>()
            .WithMany(t => t.CreditTransactions)
            .HasForeignKey(t => t.CreditedAccountId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Account>()
            .WithMany(t => t.DebitTransactions)
            .HasForeignKey(t => t.DebitedAccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}