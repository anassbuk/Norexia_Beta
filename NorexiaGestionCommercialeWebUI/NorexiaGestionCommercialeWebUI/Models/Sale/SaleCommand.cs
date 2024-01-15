using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Sale;
public class SaleCommand
{
    public Guid Id { get; set; }
    public SaleOperationType? OperationType { get; set; }
    public SaleExecution? Execution { get; set; }
    public SaleOrderOrigin? SaleOrderOrigin { get; set; }
    public Guid? QuotationId { get; set; }
    public string? QuotationRef { get; set; }
    public string? Reference { get; set; }
    public Guid? CustomerId { get; set; }
    public float? Discount { get; set; }
    public DateTimeOffset? DeliveryDate { get; set; } = DateTime.Now;
    public DateTimeOffset? OrderDate { get; set; } = DateTime.Now;
    public string? Status { get; set; }
    public string? Note { get; set; }
    public DeliveryMode? DeliveryMode { get; set; }
    public ICollection<SaleOrderLineDto>? SaleOrderLines { get; set; }
    public Guid? SaleChannelId { get; set; }
    public string? CreatedBy { get; set; }
    public OwnedPaymentTerms? PaymentTerms { get; set; }
    public ICollection<PaymentDto>? SalePayments { get; set; }
}
