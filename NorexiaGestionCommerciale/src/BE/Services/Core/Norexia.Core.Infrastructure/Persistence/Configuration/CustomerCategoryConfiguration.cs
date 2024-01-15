using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.CustomerEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class CustomerCategoryConfiguration : IEntityTypeConfiguration<CustomerCategory>
{
    public void Configure(EntityTypeBuilder<CustomerCategory> builder)
    {
        builder.ToTable(nameof(CustomerCategory), schema: "app");

        builder
            .HasKey(t => t.Id);

        builder
            .Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(128);
    }
}
