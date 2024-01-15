using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.DeliveryEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class DelivererConfiguration : IEntityTypeConfiguration<Deliverer>
{
    public void Configure(EntityTypeBuilder<Deliverer> builder)
    {
        builder.ToTable(nameof(Deliverer), schema: "app");

        builder
        .HasKey(x => x.Id);

        builder.Property(t => t.Reference)
            .IsRequired();

        builder.Property(t => t.Type)
            .IsRequired();

        builder.Property(t => t.LastName)
           .HasMaxLength(100)
           .IsRequired();
        builder.Property(t => t.FirstName)
           .HasMaxLength(100)
           .IsRequired();

        builder.Property(t => t.Tel)
           .HasMaxLength(20)
           .IsRequired();
    }
}
