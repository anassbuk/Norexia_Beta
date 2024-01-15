using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.QuotationEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;

internal class QuotationLineConfiguration : IEntityTypeConfiguration<QuotationLine>
{
    public void Configure(EntityTypeBuilder<QuotationLine> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
          .Property(t => t.Id)
              .ValueGeneratedNever();

        builder
            .Property(t => t.Qty)
            .IsRequired();

        builder
            .HasOne(t => t.SellingPrice);

        builder
            .HasOne(t => t.Quotation);


        builder
            .HasOne(t => t.Product);
    }
}
