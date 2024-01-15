using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Invoice;
public class InvoiceCommand
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerRef { get; set; }
    public string? DeliveryRef { get; set; }
    public Guid? SaleOrderId { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? SaleOrderRef { get; set; }
    public float? Discount { get; set; }
    public InvoiceOrigin? InvoiceOrigin { get; set; }
    public InvoiceType? InvoiceType { get; set; }
    public DateTimeOffset? EntryDate { get; set; } = DateTime.Now;
    public DateTimeOffset? DueDate { get; set; }
    public DateTimeOffset? DeliveryStartDate { get; set; }
    public DateTimeOffset? DeliveryEndDate { get; set; }
    public string? Status { get; set; }
    public string? DirectCreationReason { get; set; }
    public string? Note { get; set; }
    public string? CreatedBy { get; set; }
    public ICollection<InvoiceLineDto>? InvoiceLines { get; set; }
    public OwnedPaymentTerms? PaymentTerms { get; set; }
    public ICollection<PaymentDto>? InvoicePayments { get; set; }
}
