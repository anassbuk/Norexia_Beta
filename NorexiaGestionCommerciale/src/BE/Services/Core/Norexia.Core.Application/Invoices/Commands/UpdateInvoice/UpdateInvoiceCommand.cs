using MediatR;

using Norexia.Core.Application.Invoices.Commands.CreateInvoice;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.Invoices.Commands.UpdateInvoice;
public class UpdateInvoiceCommand : IRequest<Guid>
{
    public Guid? Id { get; set; }
    public string? Reference { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? SaleOrderId { get; set; }
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
    public string? Status { get; set; }
    public string? DirectCreationReason { get; set; }
    public string? Note { get; set; }
    public ICollection<InvoiceLineCommand>? InvoiceLines { get; set; }
    public ICollection<InvoicePaymentCommand>? InvoicePayments { get; set; }
}
