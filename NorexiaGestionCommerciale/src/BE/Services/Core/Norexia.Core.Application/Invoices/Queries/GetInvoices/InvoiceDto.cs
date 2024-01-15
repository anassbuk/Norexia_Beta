using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.InvoiceEntities;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.Invoices.Queries.GetInvoices;
public class InvoiceDto : IMapFrom<Invoice>
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerRef { get; set; }
    public Guid? SaleOrderId { get; set; }
    public string? SaleOrderRef { get; set; }
    public string? DeliveryRef { get; set; }
    public Guid? CurrencyId { get; set; }
    public float? Discount { get; set; }
    public InvoiceOrigin InvoiceOrigin { get; set; }
    public InvoiceType InvoiceType { get; set; }
    public OwnedPaymentTerms? PaymentTerms { get; set; }
    public DateTime EntryDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? DeliveryStartDate { get; set; }
    public DateTime? DeliveryEndDate { get; set; }
    public string? DirectCreationReason { get; set; }
    public string? Status { get; set; }
    public double? SettledAmount { get; set; }
    public double? RemainingAmount { get; set; }
    public double? TotalPriceIncludingVAT { get; set; }
    public string? Note { get; set; }
}
