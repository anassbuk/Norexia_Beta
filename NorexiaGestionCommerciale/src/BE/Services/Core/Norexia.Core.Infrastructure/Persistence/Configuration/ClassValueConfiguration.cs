using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;

public class ClassValueConfiguration : IEntityTypeConfiguration<ClassValue>
{
    public void Configure(EntityTypeBuilder<ClassValue> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(t => t.Value)
            .HasMaxLength(225)
            .IsRequired();

        builder
            .HasMany(t => t.Products);

        builder
            .HasOne(t => t.ProductClassKey);
    }
}
