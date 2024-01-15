using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.StockEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class StockEntryConfiguration : IEntityTypeConfiguration<StockEntry>
{
    public void Configure(EntityTypeBuilder<StockEntry> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(t => t.Reference)
            .HasMaxLength(225)
            .IsRequired();

        builder
            .HasOne(t => t.Provider);

        builder
            .HasOne(t => t.PurchaseOrder);

        builder
            .HasMany(t => t.StockEntryLines);
    }
}
