using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;

public class PaymentTermsConfiguration : IEntityTypeConfiguration<PaymentTerms>
{
    public void Configure(EntityTypeBuilder<PaymentTerms> builder)
    {
        builder
            .HasKey(x => x.Id);

        //builder.Property(x => x.MaturityDuration)
    }
}
