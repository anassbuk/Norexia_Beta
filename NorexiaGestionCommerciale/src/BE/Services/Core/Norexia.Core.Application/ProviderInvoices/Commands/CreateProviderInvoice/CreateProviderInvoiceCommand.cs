using MediatR;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.ProviderInvoices.Commands.CreateProviderInvoice;
public class CreateProviderInvoiceCommand : IRequest<Guid>
{
    public string? Reference { get; set; }
    public Guid? ProviderId { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public Guid? CurrencyId { get; set; }
    public float? Discount { get; set; }
    public ProviderInvoiceOrigin ProviderInvoiceOrigin { get; set; }
    public DateTime EntryDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Status { get; set; }
    public string? DirectCreationReason { get; set; }
    public string? Note { get; set; }
    public OwnedPaymentTerms? PaymentTerms { get; set; }
    public ICollection<ProviderInvoiceLineCommand>? ProviderInvoiceLines { get; set; }
    public ICollection<AttachedDigitalInvoiceCommand>? AttachedDigitalInvoices { get; set; }
    public ICollection<ProviderInvoicePaymentCommand>? ProviderInvoicePayments { get; set; }
}
