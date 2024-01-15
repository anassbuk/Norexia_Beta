using Norexia.Core.Application.Deliveries.Queries.GetDeliveryLines;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.SaleOrders.Queries.GetSaleOrderAsDelivery;
public class SaleOrderAsDeliveryDto
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerRef { get; set; }
    public DateTime? PlannedDate { get; set; }
    public DeliveryMode? DeliveryMode { get; set; }
    public virtual ICollection<DeliveryLineDto>? DeliveryLines { get; set; }
}
