using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;

public class PaymentMeanConfiguration : IEntityTypeConfiguration<PaymentMean>
{
    public void Configure(EntityTypeBuilder<PaymentMean> builder)
    {
        builder.ToTable(nameof(PaymentMean), schema: "app");

        builder
            .HasKey(x => x.Id);

        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
