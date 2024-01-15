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
    public class CreditNoteConfiguration : IEntityTypeConfiguration<CreditNote>
    {
        public void Configure(EntityTypeBuilder<CreditNote> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(t => t.CreditNumber)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(t => t.CreditNoteDate)
                .IsRequired();


            builder
                .HasOne(t => t.Invoice)
                .WithMany()
                .HasForeignKey(t => t.InvoiceId);
                

            builder
                .HasOne(t => t.Customer)
                .WithMany()
                .HasForeignKey(t => t.CustomerId);
                

            builder
                .Property(t => t.Responsable)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(t => t.Raison)
                .HasMaxLength(200)
                .IsRequired(false);

            builder
                .Property(t => t.Note)
                .HasMaxLength(200)
                .IsRequired(false);

            builder
                .Property(t => t.CreditAmount)
                .HasDefaultValue(0.0);

            builder
                .HasMany(t => t.CreditNoteLines); 

            builder
                .Property(t=>t.DueDate)
                .IsRequired(false);





        }
    }
}
