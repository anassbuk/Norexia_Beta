using MediatR;

using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.Quotations.Commands.CreateQuotation;

public class CreateQuotationCommand : IRequest<Guid>
{
    public string? Reference { get; set; }
    public DateTime? QuotationDate { get; set; }
    public int? ValidityDuretion { get; set; }
    public string? Responsable { get; set; }
    public Guid? CustomerId { get; set; }
    public string? Status { get; set; }
    public float? Discount { get; set; }
    public string? Note { get; set; }
    public int? Version { get; set; }
    public OwnedPaymentTerms? PaymentTerms { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DeliveryMode DeliveryMode { get; set; }
    public ICollection<QuotationLineCommand>? QuotationLines { get; set; }

}
