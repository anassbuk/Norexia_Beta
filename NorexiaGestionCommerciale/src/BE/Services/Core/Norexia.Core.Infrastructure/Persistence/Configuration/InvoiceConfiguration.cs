using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.InvoiceEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(t => t.Reference)
            .HasMaxLength(225)
            .IsRequired();

        builder
            .HasOne(t => t.Customer);

        builder
            .HasOne(t => t.SaleOrder);

        builder
            .HasMany(t => t.InvoiceLines);

        builder
            .HasOne(t => t.Currency);

        builder
            .HasMany(t => t.InvoicePayments);
    }
}
