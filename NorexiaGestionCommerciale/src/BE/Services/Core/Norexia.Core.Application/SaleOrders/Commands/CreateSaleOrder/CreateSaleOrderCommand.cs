using MediatR;

using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.SaleOrders.Commands.CreateSaleOrder;
public class CreateSaleOrderCommand : IRequest<Guid>
{
    public SaleOperationType OperationType { get; set; }
    public SaleExecution Execution { get; set; }
    public Guid? CustomerId { get; set; }
    public SaleOrderOrigin SaleOrderOrigin { get; set; }
    public Guid? QuotationId { get; set; }
    public string? Reference { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public float? Discount { get; set; }
    public string? Note { get; set; }
    public DeliveryMode DeliveryMode { get; set; }
    public ICollection<SaleOrderLineCommand>? SaleOrderLines { get; set; }
    public Guid? SaleChannelId { get; set; }
    public string? Status { get; set; }
    public OwnedPaymentTerms? PaymentTerms { get; set; }
    public ICollection<SaleOrderPaymentCommand>? SalePayments { get; set; }
}
