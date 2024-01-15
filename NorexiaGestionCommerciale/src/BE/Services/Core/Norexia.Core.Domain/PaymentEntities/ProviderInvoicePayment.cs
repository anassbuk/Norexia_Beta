using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.ProviderInvoiceEntities;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Domain.PaymentEntities;
public class ProviderInvoicePayment : BaseAuditableEntity
{
    public string? Reference { get; set; }
    public Guid? ProviderInvoiceId { get; set; }
    public virtual ProviderInvoice? ProviderInvoice { get; set; }
    public Guid? PaymentMeanId { get; set; }
    public virtual PaymentMean? PaymentMean { get; set; }
    public DateTime EntryDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? OperationDate { get; set; }
    public string? OperationNumber { get; set; }
    public string? Bank { get; set; }
    public string? Status { get; set; }
    public string? Note { get; set; }
    public double? AmountToBePaid { get; set; }
    public double? AmountToBePaidPercentage { get; set; }
    public double? AmountPaid { get; set; }
}
