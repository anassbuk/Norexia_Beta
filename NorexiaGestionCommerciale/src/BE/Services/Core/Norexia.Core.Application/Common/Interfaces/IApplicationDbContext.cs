using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

namespace Norexia.Core.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    public DbSet<Product> Products { get; }
    public DbSet<ProductImage> ProductImages { get; }
    public DbSet<ProductNote> ProductNotes { get; }
    public DbSet<Family> Families { get; }
    public DbSet<UnitMeasurement> UnitMeasurements { get; }
    public DbSet<UnitType> UnitTypes { get; }
    public DbSet<ClassValue> ClassValues { get; }
    public DbSet<ClassKey> ClassKeys { get; }
    public DbSet<SellingPrice> SellingPrices { get; }
    public DbSet<PriceGroup> PriceGroups { get; }
    public DbSet<Customer> Customers { get; }
    public DbSet<CustomerCategory> CustomerCategories { get; }
    public DbSet<CustomerAddress> Addresses { get; }
    public DbSet<Provider> Providers { get; }
    public DbSet<ProviderAddress> ProviderAddress { get; }
    public DbSet<SaleOrder> SaleOrders { get; }
    public DbSet<SaleOrderLine> SaleOrderLines { get; }
    public DbSet<ProductAvailability> ProductAvailabilities { get; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; }
    public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; }
    public DbSet<ProviderCategory> ProviderCategories { get; }
    public DbSet<StockEntry> StockEntries { get; }
    public DbSet<StockEntryLine> StockEntryLines { get; }
    public DbSet<Deliverer> Deliverers { get; }
    public DbSet<Delivery> Deliveries { get; }
    public DbSet<DeliveryLine> DeliveryLines { get; }
    public DbSet<PaymentMean> PaymentMeans { get; }
    public DbSet<Domain.SaleOrderEntities.PaymentTerms> PaymentTerms { get; }
    public DbSet<VAT> VATs { get; }
    public DbSet<Currency> Currencies { get; }
    public DbSet<Invoice> Invoices { get; }
    public DbSet<InvoiceLine> InvoiceLines { get; }
    public DbSet<Quotation> Quotations { get; }
    public DbSet<QuotationLine> QuotationsLines { get; }

    public DbSet<CreditNote> CreditNotes { get; }
    public DbSet<CreditNoteLine> CreditNoteLines { get; }


    public DbSet<InvoicePayment> InvoicePayments { get; }
    public DbSet<SalePayment> SalePayments { get; }
    public DbSet<ProviderInvoice> ProviderInvoices { get; }
    public DbSet<ProviderInvoiceLine> ProviderInvoiceLines { get; }
    public DbSet<ProviderInvoicePayment> ProviderInvoicePayments { get; }
    public DbSet<AttachedDigitalInvoice> AttachedDigitalInvoices { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}
