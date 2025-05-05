using System.Collections.Immutable;
using EfCoreDemo.Domain;
using EfCoreDemo.Infrastructure.Conversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
                .HasConversion<string?>(
                    mn => string.Join(MiddleNameSeparator, mn.Select(x => x.Value)),
                    mn => String.IsNullOrEmpty(mn)
                        ? ImmutableList<GivenName>.Empty
                        : mn.Split(MiddleNameSeparator, StringSplitOptions.None).Select(GivenName.From)
                            .ToImmutableList())
                .Metadata.SetValueComparer(new ValueComparer<ImmutableList<GivenName>>(
                    (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c));
        });

        builder.OwnsOne(c => c.Addresses, addressesBuilder =>
        {
            addressesBuilder.Ignore(a => a.IsSameAsResidential);
            addressesBuilder.OwnsOne(a => a.Residential, ConfigureAddress);
            addressesBuilder.OwnsOne<Address>("PostalInternal", ConfigureAddress);
        });

        builder
            .HasMany(c => c.Accounts)
            .WithOne()
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(c => c.ContactDetails, contactBuilder =>
        {
            contactBuilder.Property(c => c.PhoneNumber)
                .HasConversion(new PhoneNumberConverter())
                .HasMaxLength(12);
            contactBuilder.Property(c => c.AlternateNumber)
                .HasConversion(new PhoneNumberConverter())
                .HasMaxLength(12);
            contactBuilder.Property(c => c.WorkNumber)
                .HasConversion(new PhoneNumberConverter())
                .HasMaxLength(12);
            contactBuilder.Property(c => c.Email).HasMaxLength(100);
        });

        builder.OwnsOne(c => c.DemographicInfo, demographicBuilder =>
        {
            demographicBuilder.Property(d => d.Gender).HasMaxLength(8);
            demographicBuilder.Property(d => d.PreferredLanguage).HasMaxLength(2);
        });

        builder.OwnsOne(c => c.MarketingPreferences);

        return;

        void ConfigureAddress(OwnedNavigationBuilder<Addresses, Address> addressBuilder)
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
        }
    }
}