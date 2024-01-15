using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.SettingEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;

public class VATConfiguration : IEntityTypeConfiguration<VAT>
{
    public void Configure(EntityTypeBuilder<VAT> builder)
    {
        builder.ToTable(nameof(VAT), schema: "app");

        builder
           .HasKey(x => x.Id);

        builder.Property(t => t.Value)
           .IsRequired();
    }
}
