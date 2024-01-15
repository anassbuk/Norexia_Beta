using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.ProductEntities;
using Norexia.Core.Domain.ProviderInvoiceEntities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class AttachedDigitalInvoiceConfiguration : IEntityTypeConfiguration<AttachedDigitalInvoice>
{
    public void Configure(EntityTypeBuilder<AttachedDigitalInvoice> builder)
    {
        builder.ToTable(nameof(AttachedDigitalInvoice), schema: "app");

        builder.HasKey(x => x.Id);

        builder.Property(t => t.InvoiceId)
            .IsRequired();

        builder.Property(t => t.Label)
            .IsRequired();

        builder.Property(t => t.Path)
            .HasMaxLength(500)
            .IsRequired();
    }
}
