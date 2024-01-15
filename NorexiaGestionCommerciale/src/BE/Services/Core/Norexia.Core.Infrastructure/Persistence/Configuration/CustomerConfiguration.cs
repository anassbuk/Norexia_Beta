﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Norexia.Core.Domain.CustomerEntities;

namespace Norexia.Core.Infrastructure.Persistence.Configuration;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(nameof(Customer), schema: "app");

        builder
            .HasKey(x => x.Id);

        builder.Property(t => t.Reference)
            .IsRequired();

        builder.Property(t => t.ClientType)
            .IsRequired();

        builder.Property(t => t.LastName)
           .HasMaxLength(100)
           .IsRequired();
        builder.Property(t => t.FirstName)
           .HasMaxLength(100)
           .IsRequired();

        /*
         TODO : Conditional configuration
         */
        //When(t => t.ClientType == ClientType.Company, () => {
        //    RuleFor(t => t.SocialReason)
        //   .MaximumLength(300)
        //   .NotEmpty();

        //    RuleFor(t => t.ICE)
        //      .MaximumLength(100);
        //});

        builder.Property(t => t.Tel)
           .HasMaxLength(20)
           .IsRequired();

        builder.Property(t => t.Fax)
           .HasMaxLength(20);

        builder.Property(t => t.Mobile)
           .HasMaxLength(20);

        builder.Property(t => t.Email)
            .HasMaxLength(200);

        builder.Property(t => t.WebSite)
           .HasMaxLength(100);

        builder.HasMany(t => t.CustomerAddresses);

        builder
            .HasOne(t => t.CustomerCategory);
    }
}
