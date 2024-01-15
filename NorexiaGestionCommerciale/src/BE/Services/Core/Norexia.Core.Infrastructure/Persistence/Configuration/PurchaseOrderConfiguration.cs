using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.PurchaseOrderEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .HasOne(t => t.Provider);

        builder
            .HasMany(t => t.PurchaseOrderLines);
    }
}
