using EfCoreDemo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreDemo.Infrastructure.Configuration;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.OwnsOne<CustomerName>(c => c.Name);
        builder
            .HasMany(c => c.Accounts)
            .WithOne()
            .HasForeignKey(a => a.CustomerId);
    }
}