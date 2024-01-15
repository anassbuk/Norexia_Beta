using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Norexia.Core.Domain.PaymentEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class ProviderInvoicePaymentConfiguration : IEntityTypeConfiguration<ProviderInvoicePayment>
{
    public void Configure(EntityTypeBuilder<ProviderInvoicePayment> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(t => t.Reference)
            .HasMaxLength(225)
            .IsRequired();

        builder
            .HasOne(t => t.PaymentMean);

        builder
            .HasOne(t => t.ProviderInvoice);

        builder.Property(t => t.EntryDate)
            .IsRequired();

        builder.Property(t => t.DueDate)
            .IsRequired();

        builder.Property(t => t.PaymentMeanId)
            .IsRequired();

        builder
            .Property(t => t.Id)
                .ValueGeneratedNever();
    }
}
