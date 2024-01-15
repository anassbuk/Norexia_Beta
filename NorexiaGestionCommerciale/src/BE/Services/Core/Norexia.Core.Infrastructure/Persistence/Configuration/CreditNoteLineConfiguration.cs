using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Norexia.Core.Domain.CreditNoteEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Infrastructure.Persistence.Configuration
{
    public class CreditNoteLineConfiguration : IEntityTypeConfiguration<CreditNoteLine>
    {
        public void Configure(EntityTypeBuilder<CreditNoteLine> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(t => t.SellingPriceId)
                .IsRequired();

            builder
                .HasOne(t => t.SellingPrice);

            builder
                .Property(t => t.DeliveryRef)
                .HasMaxLength(50)
                .IsRequired(false);


            builder
                .Property(t => t.ProductId)
                .IsRequired();

            builder
                .HasOne(t => t.Product);

            builder
               .HasOne(t => t.SellingPrice);



            builder
                .Property(t => t.Price)
                .IsRequired();

            builder
                .Property(t => t.VAT)
                .IsRequired();

            builder
                .Property(t => t.Discount)
                .IsRequired(false);

            builder
                .Property(t => t.Qty)
                .IsRequired();

            builder
                .Property(t => t.ExpectedQty)
                .IsRequired(false);


            builder
               .HasOne(t => t.CreditNote);



        }
    }
}
