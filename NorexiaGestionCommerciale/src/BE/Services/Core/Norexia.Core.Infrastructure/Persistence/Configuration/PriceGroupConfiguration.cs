using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;

public class PriceGroupConfiguration : IEntityTypeConfiguration<PriceGroup>
{
    public void Configure(EntityTypeBuilder<PriceGroup> builder)
    {
        builder.ToTable(nameof(PriceGroup), schema: "app");

        builder
            .HasKey(x => x.Id);

        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
