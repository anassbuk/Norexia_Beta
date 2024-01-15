using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class SaleOrderConfiguration : IEntityTypeConfiguration<SaleOrder>
{
    public void Configure(EntityTypeBuilder<SaleOrder> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(t => t.SaleOrderOrigin)
            .IsRequired();

        builder
            .HasOne(t => t.Customer);

        builder
            .HasOne(t => t.SaleChannel);

        builder
            .HasMany(t => t.SaleOrderLines);

        builder
            .OwnsOne(t => t.PaymentTerms);

        builder
            .HasMany(t => t.SalePayments);

        builder
            .HasOne(t => t.Quotation);
    }
}
