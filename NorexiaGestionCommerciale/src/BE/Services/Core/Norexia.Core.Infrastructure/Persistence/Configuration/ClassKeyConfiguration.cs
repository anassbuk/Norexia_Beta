using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;

public class ClassKeyConfiguration : IEntityTypeConfiguration<ClassKey>
{
    public void Configure(EntityTypeBuilder<ClassKey> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(t => t.Key)
            .HasMaxLength(225)
            .IsRequired();

        builder
            .HasMany(t => t.Values);
    }
}
