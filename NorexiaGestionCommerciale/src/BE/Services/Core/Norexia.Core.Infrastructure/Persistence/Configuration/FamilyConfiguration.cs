using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class FamilyConfiguration : IEntityTypeConfiguration<Family>
{
    public void Configure(EntityTypeBuilder<Family> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(t => t.Designation)
            .HasMaxLength(225)
            .IsRequired();

        builder
        .HasMany(f => f.SubFamilies)
        .WithOne(f => f.ParentFamily)
        .HasForeignKey(answer => answer.ParentFamilyId);
    }
}
