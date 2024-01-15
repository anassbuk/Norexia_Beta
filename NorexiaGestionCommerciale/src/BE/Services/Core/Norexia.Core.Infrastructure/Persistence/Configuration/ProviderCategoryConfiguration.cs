using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.ProviderEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class ProviderCategoryConfiguration : IEntityTypeConfiguration<ProviderCategory>
{
    public void Configure(EntityTypeBuilder<ProviderCategory> builder)
    {
        builder.ToTable(nameof(ProviderCategory), schema: "app");

        builder
            .HasKey(t => t.Id);

        builder
            .Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(128);
    }
}