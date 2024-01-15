using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.PaymentEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class InvoicePaymentConfiguration : IEntityTypeConfiguration<InvoicePayment>
{
    public void Configure(EntityTypeBuilder<InvoicePayment> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(t => t.Reference)
            .HasMaxLength(225)
            .IsRequired();

        builder
            .HasOne(t => t.PaymentMean);

        builder
            .HasOne(t => t.Invoice);

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
