using System.Collections.Immutable;
using EfCoreDemo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreDemo.Infrastructure.Configuration;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    private const char MiddleNameSeparator = ' ';
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.OwnsOne<CustomerName>(c => c.Name, customerNameBuilder =>
        {
            customerNameBuilder.Property(cn => cn.First).HasMaxLength(50);
            customerNameBuilder.Property(cn => cn.Last).HasMaxLength(50);
            customerNameBuilder.Property(cn => cn.MiddleNames)
                .IsRequired(false)
                .HasMaxLength(250)
                .HasConversion(
                    mn => string.Join(MiddleNameSeparator, mn.Select(x => x.Value)), 
                    mn => mn.Split(MiddleNameSeparator, StringSplitOptions.None).Select(GivenName.From).ToImmutableList());
        });
        
        builder.OwnsOne<Address>(c => c.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.PostalCode).HasMaxLength(4);
            addressBuilder.Property(a => a.Province).HasMaxLength(20);
            addressBuilder.Property(a => a.Suburb).HasMaxLength(50);
            addressBuilder.Property(a => a.City).HasMaxLength(50);
            addressBuilder.OwnsOne(a => a.Street, streetBuilder =>
            {
                streetBuilder.Property(s => s.FirstLine).HasMaxLength(50);
                streetBuilder.Property(s => s.SecondLine).HasMaxLength(50);
            });
        });
        
        builder
            .HasMany(c => c.Accounts)
            .WithOne()
            .HasForeignKey(a => a.CustomerId);
    }
}