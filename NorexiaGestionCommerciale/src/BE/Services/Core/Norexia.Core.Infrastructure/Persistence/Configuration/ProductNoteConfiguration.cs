using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class ProductNoteConfiguration : IEntityTypeConfiguration<ProductNote>
{
    public void Configure(EntityTypeBuilder<ProductNote> builder)
    {
        builder.ToTable(nameof(ProductNote), schema: "app");

        builder.HasKey(x => x.Id);


        builder.Property(t => t.Note)
            .HasMaxLength(2500)
            .IsRequired();

    }
}
