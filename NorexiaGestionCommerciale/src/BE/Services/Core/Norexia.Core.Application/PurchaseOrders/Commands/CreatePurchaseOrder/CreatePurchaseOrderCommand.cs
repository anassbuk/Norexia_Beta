using MediatR;

namespace Norexia.Core.Application.PurchaseOrders.Commands.CreatePurchaseOrder;
public class CreatePurchaseOrderCommand : IRequest<Guid>
{
    public string? Reference { get; set; }
    public Guid ProviderId { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? Status { get; set; }
    public string? Note { get; set; }
    public ICollection<PurchaseOrderLineCommand>? PurchaseOrderLines { get; set; }
}
