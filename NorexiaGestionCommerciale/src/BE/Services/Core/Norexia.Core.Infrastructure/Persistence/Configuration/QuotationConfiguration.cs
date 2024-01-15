using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.QuotationEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;

internal class QuotationConfiguration : IEntityTypeConfiguration<Quotation>
{
    public void Configure(EntityTypeBuilder<Quotation> builder)
    {
        builder
            .HasKey(x => x.Id);

        //builder
        //    .Property(t => t.QuotationLines)
        //    .IsRequired();

        builder
            .HasOne(t => t.Customer);
        

        builder
            .HasMany(t => t.QuotationLines);

        builder
            .OwnsOne(t => t.PaymentTerms);
    }
}

