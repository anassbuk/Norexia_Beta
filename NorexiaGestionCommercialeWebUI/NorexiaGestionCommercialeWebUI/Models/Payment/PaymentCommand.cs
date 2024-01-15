using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Payment;
public class PaymentCommand
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? InvoiceId { get; set; }
    public string? InvoiceRef { get; set; }
    public Guid? SaleOrderId { get; set; }
    public string? SaleOrderRef { get; set; }
    public Guid? PaymentMeanId { get; set; }
    public DateTimeOffset? EntryDate { get; set; } = DateTime.Now;
    public DateTimeOffset? DueDate { get; set; }
    public DateTimeOffset? OperationDate { get; set; }
    public PaymentOrigin? PaymentOrigin { get; set; }
    public string? OperationNumber { get; set; }
    public string? Bank { get; set; }
    public string? Status { get; set; }
    public string? Note { get; set; }
    public double? AmountToBePaid { get; set; }
    public float? AmountToBePaidPercentage { get; set; }
    public double? AmountPaid { get; set; }
}
