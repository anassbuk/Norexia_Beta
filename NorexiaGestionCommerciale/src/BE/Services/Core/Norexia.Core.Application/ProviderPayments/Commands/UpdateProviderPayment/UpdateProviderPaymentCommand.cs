using MediatR;

namespace Norexia.Core.Application.ProviderPayments.Commands.UpdateProviderPayment;
public class UpdateProviderPaymentCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? ProviderInvoiceId { get; set; }
    public Guid? PaymentMeanId { get; set; }
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
