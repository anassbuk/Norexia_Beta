using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;

public class SellingPriceConfiguration : IEntityTypeConfiguration<SellingPrice>
{
    public void Configure(EntityTypeBuilder<SellingPrice> builder)
    {
        builder.ToTable(nameof(SellingPrice), schema: "app");

        builder
            .HasKey(x => x.Id);

        builder
            .HasOne(f => f.PriceGroup);


        builder.Property(t => t.Price)
            .IsRequired();
    }
}
