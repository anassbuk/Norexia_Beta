using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(nameof(Product), schema: "app");

        builder
            .HasKey(x => x.Id);

        builder.Property(t => t.ShortDesignation)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.LongDesignation)
            .HasMaxLength(1000);

        builder.Property(t => t.Description)
            .HasMaxLength(2500);

        builder.OwnsOne(t => t.ClassificationInfo)
            .HasOne(t => t.Family);

        builder.OwnsOne(t => t.UnitInfo);
        builder.OwnsOne(t => t.SellInfo);
        builder.OwnsOne(t => t.StorageSupplyInfo);
        builder.OwnsOne(t => t.PurchaseInfo);

        builder.HasMany(t => t.ProductClassValues)
            .WithMany(t => t.Products)
            .UsingEntity("ProductClassValues",
                l => l.HasOne(typeof(ClassValue)).WithMany().HasForeignKey("ClassValueForeignKey"),
                r => r.HasOne(typeof(Product)).WithMany().HasForeignKey("ProductForeignKey"));

        builder.HasMany(t => t.ProductUnitMeasurements)
            .WithMany(t => t.Products)
            .UsingEntity("ProductUnitMeasurements",
                l => l.HasOne(typeof(UnitMeasurement)).WithMany().HasForeignKey("UnitMeasurementForeignKey"),
                r => r.HasOne(typeof(Product)).WithMany().HasForeignKey("ProductForeignKey"));

        builder
            .HasMany(t => t.SellingPrices);

        builder.HasMany(t => t.ProductAvailabilities)
            .WithMany(t => t.Products)
            .UsingEntity("ProductAssignedAvailabilities",
                l => l.HasOne(typeof(ProductAvailability)).WithMany().HasForeignKey("ProductAvailabilityForeignKey"),
                r => r.HasOne(typeof(Product)).WithMany().HasForeignKey("ProductForeignKey"));

    }
}
