using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.PurchaseOrderEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class PurchaseOrderLineConfiguration : IEntityTypeConfiguration<PurchaseOrderLine>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderLine> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(t => t.Qty)
            .IsRequired();

        builder
            .HasOne(t => t.Product);

        builder
            .HasOne(t => t.PurchaseOrder);
    }
}
