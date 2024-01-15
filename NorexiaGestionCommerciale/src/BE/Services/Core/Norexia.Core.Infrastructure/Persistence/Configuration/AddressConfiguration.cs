using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.CustomerEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;

public class AddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> builder)
    {
        builder.ToTable(nameof(CustomerAddress), schema: "app");

        builder
           .HasKey(x => x.Id);

        builder.Property(t => t.AddressType)
           .IsRequired();

        builder.Property(t => t.StreetAdress)
           .HasMaxLength(200)
           .IsRequired();

        builder.Property(t => t.City)
           .HasMaxLength(100)
           .IsRequired();

        builder.Property(t => t.ZipCode)
            .HasMaxLength(20);

        builder.Property(t => t.Region)
        .HasMaxLength(50);

        builder.Property(t => t.Complement)
       .HasMaxLength(100);

        builder.Property(t => t.Localisation)
       .HasMaxLength(100);
    }
}
