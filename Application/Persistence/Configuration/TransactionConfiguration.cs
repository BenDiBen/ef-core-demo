using EfCoreDemo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreDemo.Persistence.Configuration;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.HasOne(t => t.CreditedAccount)
            .WithMany(t => t.CreditTransactions)
            .HasForeignKey(t => t.CreditedAccountId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(t => t.DebitedAccount)
            .WithMany(t => t.DebitTransactions)
            .HasForeignKey(t => t.DebitedAccountId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(t => t.CreditorReference).HasMaxLength(200);
        builder.Property(t => t.DebtorReference).HasMaxLength(200);
    }
}