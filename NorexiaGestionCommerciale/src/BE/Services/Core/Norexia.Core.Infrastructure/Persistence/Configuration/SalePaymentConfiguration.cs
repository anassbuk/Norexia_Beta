using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Domain.PaymentEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class SalePaymentConfiguration : IEntityTypeConfiguration<SalePayment>
{
    public void Configure(EntityTypeBuilder<SalePayment> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(t => t.Reference)
            .HasMaxLength(225)
            .IsRequired();

        builder
            .HasOne(t => t.PaymentMean);

        builder
            .HasOne(t => t.SaleOrder);

        builder.Property(t => t.EntryDate)
            .IsRequired();

        builder.Property(t => t.DueDate)
            .IsRequired(false);

        builder.Property(t => t.PaymentMeanId)
            .IsRequired();


        builder
            .Property(t => t.Id)
                .ValueGeneratedNever();
    }
}
