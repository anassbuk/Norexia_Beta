using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class UnitMeasurementConfiguration : IEntityTypeConfiguration<UnitMeasurement>
{

    public void Configure(EntityTypeBuilder<UnitMeasurement> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(t => t.Name)
            .HasMaxLength(125)
            .IsRequired();

        builder
        .HasMany(f => f.Products);

        builder
        .HasOne(f => f.UnitType);

    }
}
