using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.StockEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class StockEntryLineConfiguration : IEntityTypeConfiguration<StockEntryLine>
{
    public void Configure(EntityTypeBuilder<StockEntryLine> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(t => t.Qty)
            .IsRequired();

        builder
            .HasOne(t => t.Product);
    }
}
