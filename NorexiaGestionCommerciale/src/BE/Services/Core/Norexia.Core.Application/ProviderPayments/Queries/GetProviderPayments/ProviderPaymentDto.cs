using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.PaymentEntities;

namespace Norexia.Core.Application.ProviderPayments.Queries.GetProviderPayments;
public class ProviderInvoicePaymentDto : IMapFrom<ProviderInvoicePayment>
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? ProviderInvoiceId { get; set; }
    public string? ProviderInvoiceRef { get; set; }
    public Guid? PaymentMeanId { get; set; }
    public string? PaymentMeanName { get; set; }
    public DateTime? EntryDate { get; set; }
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
