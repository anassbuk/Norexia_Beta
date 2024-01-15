using MediatR;

using Norexia.Core.Application.PurchaseOrders.Commands.CreatePurchaseOrder;

namespace Norexia.Core.Application.PurchaseOrders.Commands.UpdatePurchaseOrder;
public class UpdatePurchaseOrderCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public Guid ProviderId { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? Status { get; set; }
    public string? Note { get; set; }
    public ICollection<PurchaseOrderLineCommand>? PurchaseOrderLines { get; set; }
}
