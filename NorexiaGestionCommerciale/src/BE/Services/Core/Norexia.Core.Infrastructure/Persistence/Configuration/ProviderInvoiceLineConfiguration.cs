using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.ProviderInvoiceEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class ProviderInvoiceLineConfiguration : IEntityTypeConfiguration<ProviderInvoiceLine>
{
    public void Configure(EntityTypeBuilder<ProviderInvoiceLine> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(t => t.Price)
            .IsRequired();

        builder
            .Property(t => t.VAT)
            .IsRequired(false);

        builder
            .Property(t => t.Discount)
            .IsRequired(false);

        builder
            .Property(t => t.Qty)
            .IsRequired(false);

        builder
            .Property(t => t.ExpectedQty)
            .IsRequired(false);

        builder
            .HasOne(t => t.ProviderInvoice);

        builder
            .HasOne(t => t.Product);
    }
}
