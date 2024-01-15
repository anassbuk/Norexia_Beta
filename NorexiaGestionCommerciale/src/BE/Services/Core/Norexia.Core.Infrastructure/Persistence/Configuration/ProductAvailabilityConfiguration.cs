using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class ProductAvailabilityConfiguration : IEntityTypeConfiguration<ProductAvailability>
{
    public void Configure(EntityTypeBuilder<ProductAvailability> builder)
    {
        builder.ToTable(nameof(ProductAvailability), schema: "app");

        builder
            .HasKey(x => x.Id);

        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .HasMany(t => t.Products);
    }
}
