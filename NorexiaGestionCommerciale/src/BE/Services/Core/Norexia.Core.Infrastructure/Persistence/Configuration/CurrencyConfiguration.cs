using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.SettingEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable(nameof(Currency), schema: "app");

        builder
           .HasKey(x => x.Id);

        builder.Property(t => t.Name)
           .IsRequired();
    }
}
