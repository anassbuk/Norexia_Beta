using MediatR;

using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.Deliveries.Commands.CreateDelivery;
public class CreateDeliveryCommand : IRequest<Guid>
{
    public string? Reference { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? DelivererId { get; set; }
    public Guid? SaleOrderId { get; set; }
    public Guid? InvoiceId { get; set; }
    public DeliveryOrigin DeliveryOrigin { get; set; }
    public DateTime EntryDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime PlannedDate { get; set; }
    public DateTime DeliveryTime { get; set; }
    public DeliveryMode? DeliveryMode { get; set; }
    public string? Status { get; set; }
    public string? Situation { get; set; }
    public StockRecordType? Type { get; set; }
    public string? Note { get; set; }
    public ICollection<DeliveryLineCommand>? DeliveryLines { get; set; }
}
