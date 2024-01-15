using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.CreditNoteEntities;
using Norexia.Core.Domain.CustomerEntities;
using Norexia.Core.Domain.DeliveryEntities;
using Norexia.Core.Domain.InvoiceEntities;
using Norexia.Core.Domain.PaymentEntities;
using Norexia.Core.Domain.ProductEntities;
using Norexia.Core.Domain.ProviderEntities;
using Norexia.Core.Domain.ProviderInvoiceEntities;
using Norexia.Core.Domain.PurchaseOrderEntities;
using Norexia.Core.Domain.QuotationEntities;
using Norexia.Core.Domain.SaleOrderEntities;
using Norexia.Core.Domain.SettingEntities;
using Norexia.Core.Domain.StockEntities;
using Norexia.Core.Infrastructure.Persistence.Interceptors;

using System.Reflection;

namespace Norexia.Core.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(
         DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor
        ) : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<ProductNote> ProductNotes => Set<ProductNote>();
    public DbSet<Family> Families => Set<Family>();
    public DbSet<UnitMeasurement> UnitMeasurements => Set<UnitMeasurement>();
    public DbSet<UnitType> UnitTypes => Set<UnitType>();
    public DbSet<ClassValue> ClassValues => Set<ClassValue>();
    public DbSet<ClassKey> ClassKeys => Set<ClassKey>();
    public DbSet<SellingPrice> SellingPrices => Set<SellingPrice>();
    public DbSet<PriceGroup> PriceGroups => Set<PriceGroup>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerCategory> CustomerCategories => Set<CustomerCategory>();
    public DbSet<CustomerAddress> Addresses => Set<CustomerAddress>();
    public DbSet<SaleOrder> SaleOrders => Set<SaleOrder>();
    public DbSet<SaleOrderLine> SaleOrderLines => Set<SaleOrderLine>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderLine> PurchaseOrderLines => Set<PurchaseOrderLine>();
    public DbSet<ProviderCategory> ProviderCategories => Set<ProviderCategory>();
    public DbSet<StockEntry> StockEntries => Set<StockEntry>();
    public DbSet<StockEntryLine> StockEntryLines => Set<StockEntryLine>();
    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<ProviderAddress> ProviderAddress => Set<ProviderAddress>();
    public DbSet<ProductAvailability> ProductAvailabilities => Set<ProductAvailability>();
    public DbSet<Deliverer> Deliverers => Set<Deliverer>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<DeliveryLine> DeliveryLines => Set<DeliveryLine>();
    public DbSet<PaymentMean> PaymentMeans => Set<PaymentMean>();
    public DbSet<PaymentTerms> PaymentTerms => Set<PaymentTerms>();
    public DbSet<VAT> VATs => Set<VAT>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();
    public DbSet<InvoicePayment> InvoicePayments => Set<InvoicePayment>();
    public DbSet<SalePayment> SalePayments => Set<SalePayment>();
    public DbSet<Quotation> Quotations => Set<Quotation>();
    public DbSet<QuotationLine> QuotationsLines => Set<QuotationLine>();
    public DbSet<ProviderInvoice> ProviderInvoices => Set<ProviderInvoice>();
    public DbSet<ProviderInvoiceLine> ProviderInvoiceLines => Set<ProviderInvoiceLine>();
    public DbSet<ProviderInvoicePayment> ProviderInvoicePayments => Set<ProviderInvoicePayment>();
    public DbSet<AttachedDigitalInvoice> AttachedDigitalInvoices => Set<AttachedDigitalInvoice>();

    public DbSet<CreditNote> CreditNotes =>Set<CreditNote>();

    public DbSet<CreditNoteLine> CreditNoteLines => Set<CreditNoteLine>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("app");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<PaymentMean>().HasData(
            new PaymentMean { Id = Guid.NewGuid(), Name = "Carte bancaire" },
            new PaymentMean { Id = Guid.NewGuid(), Name = "Virement" },
            new PaymentMean { Id = Guid.NewGuid(), Name = "Chèque" }
        );

        builder.Entity<VAT>().HasData(
            new VAT { Id = Guid.NewGuid(), Value = 0.02M, IsDefault = true }
        );

        builder.Entity<Currency>().HasData(
            new Currency { Id = Guid.NewGuid(), Name = "MAD", IsDefault = true }
        );

        builder.Entity<PaymentTerms>().HasData(
            new PaymentTerms
            {
                Id = Guid.NewGuid(),
                MaturityDuration = 30,
                MaturityDurationNegotiable = true,
                DepositInvoice = true,
                DepositInvoiceNegotiable = true,
                DepositInvoiceDownPayment = 10,
                PaymentByInstallments = true,
                PaymentByInstallmentsNegotiable = true,
                PaymentByInstallmentsNumber = 3,
                PaymentOption = PaymentOption.CashOnDelivery,
            }
        );

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);

        optionsBuilder.ConfigureWarnings(warn => warn.Ignore(CoreEventId.DetachedLazyLoadingWarning));
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
