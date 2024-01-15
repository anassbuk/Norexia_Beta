using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.ProviderInvoiceEntities;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.ProviderInvoices.Queries.GetProviderInvoices;
public class ProviderInvoiceDto : IMapFrom<ProviderInvoice>
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? ProviderId { get; set; }
    public string? ProviderRef { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public string? PurchaseOrderRef { get; set; }
    public Guid? CurrencyId { get; set; }
    public ProviderInvoiceOrigin ProviderInvoiceOrigin { get; set; }
    public DateTime EntryDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string? DirectCreationReason { get; set; }
    public string? Status { get; set; }
    public double? SettledAmount { get; set; }
    public double? RemainingAmount { get; set; }
    public double? TotalPriceIncludingVAT { get; set; }
    public string? Note { get; set; }
    public string? DigitalInvoicePath { get; set; }
    public OwnedPaymentTerms? PaymentTerms { get; set; }
}
