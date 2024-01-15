using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.ProviderPayments.Commands.CreateProviderPayment;
public class CreateProviderPaymentCommand : IRequest<Guid>
{
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
