using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.DeliveryEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class DeliveryLineConfiguration : IEntityTypeConfiguration<DeliveryLine>
{
    public void Configure(EntityTypeBuilder<DeliveryLine> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(t => t.Qty)
            .IsRequired();

        builder
            .Property(t => t.UnitPrice)
            .IsRequired();

        builder
            .Property(t => t.VAT)
            .IsRequired();

        builder
            .HasOne(t => t.Product);

        builder
            .HasOne(t => t.SellingPrice);

        builder
            .HasOne(t => t.Delivery);
    }
}
