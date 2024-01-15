using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.InvoiceEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
internal class InvoiceLineConfiguration : IEntityTypeConfiguration<InvoiceLine>
{
    public void Configure(EntityTypeBuilder<InvoiceLine> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(t => t.Qty)
            .IsRequired();

        builder
            .Property(t => t.Price)
            .IsRequired();

        builder
            .Property(t => t.VAT)
            .IsRequired();

        builder
            .HasOne(t => t.Invoice);

        builder
            .HasOne(t => t.Product);

        builder
            .HasOne(t => t.SellingPrice);
    }
}
