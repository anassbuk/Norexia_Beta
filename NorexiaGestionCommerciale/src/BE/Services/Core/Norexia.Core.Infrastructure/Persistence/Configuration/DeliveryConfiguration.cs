using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.DeliveryEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(t => t.Reference)
            .HasMaxLength(225)
            .IsRequired();

        builder
            .HasOne(t => t.Customer);

        builder
            .HasOne(t => t.SaleOrder);

        builder
            .HasOne(t => t.Invoice);

        builder
            .HasMany(t => t.DeliveryLines);
    }
}
