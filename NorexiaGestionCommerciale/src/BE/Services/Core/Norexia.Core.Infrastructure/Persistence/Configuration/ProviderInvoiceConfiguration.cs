using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.ProviderInvoiceEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class ProviderInvoiceConfiguration : IEntityTypeConfiguration<ProviderInvoice>
{
    public void Configure(EntityTypeBuilder<ProviderInvoice> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(t => t.Reference)
            .HasMaxLength(225)
            .IsRequired();

        builder.Property(t => t.Discount)
            .IsRequired(false);

        builder.Property(t => t.ProviderInvoiceOrigin)
            .IsRequired();

        builder.Property(t => t.EntryDate)
            .IsRequired();

        builder.Property(t => t.DueDate)
            .IsRequired(false);

        builder.Property(t => t.Status)
            .IsRequired(false);

        builder.Property(t => t.DirectCreationReason)
            .IsRequired(false);

        builder.Property(t => t.Note)
            .IsRequired(false);

        builder
            .OwnsOne(t => t.PaymentTerms);

        builder
            .HasOne(t => t.Provider);

        builder
            .HasOne(t => t.PurchaseOrder);

        builder
            .HasMany(t => t.ProviderInvoiceLines);

        builder
            .HasMany(t => t.AttachedDigitalInvoices);

        builder
            .HasMany(t => t.ProviderInvoicePayments);

        builder
            .HasOne(t => t.Currency);
    }
}
