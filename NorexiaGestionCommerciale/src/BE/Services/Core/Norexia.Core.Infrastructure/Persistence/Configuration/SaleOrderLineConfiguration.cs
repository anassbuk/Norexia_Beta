using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class SaleOrderLineConfiguration : IEntityTypeConfiguration<SaleOrderLine>
{
    public void Configure(EntityTypeBuilder<SaleOrderLine> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(t => t.Id)
                .ValueGeneratedNever();

        builder
            .Property(t => t.Qty)
            .IsRequired();

        builder
            .HasOne(t => t.SellingPrice);

        builder
            .HasOne(t => t.SaleOrder);

        builder
            .HasOne(t => t.Product);
    }
}
