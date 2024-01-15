using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.PaymentEntities;

namespace Norexia.Core.Application.Payments.Queries.GetPayments;
public class PaymentDto : IMapFrom<InvoicePayment>, IMapFrom<SalePayment>
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? InvoiceId { get; set; }
    public string? InvoiceRef { get; set; }
    public Guid? SaleOrderId { get; set; }
    public string? SaleOrderRef { get; set; }
    public Guid? PaymentMeanId { get; set; }
    public string? PaymentMeanName { get; set; }
    public DateTime? EntryDate { get; set; }
    public PaymentOrigin? PaymentOrigin { get; set; }
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
