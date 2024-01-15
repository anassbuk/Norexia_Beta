using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class UnitTypeConfiguration : IEntityTypeConfiguration<UnitType>
{
    public void Configure(EntityTypeBuilder<UnitType> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(t => t.Name)
            .HasMaxLength(125)
            .IsRequired();

        builder
        .HasMany(f => f.Measurements);
    }
}
