using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.DeliveryEntities;

namespace Norexia.Core.Application.Deliveries.Queries.GetDeliveries;
public class DeliveryDto : IMapFrom<Delivery>
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerRef { get; set; }
    public Guid? DelivererId { get; set; }
    public string? DelivererRef { get; set; }
    public Guid? SaleOrderId { get; set; }
    public string? SaleOrderRef { get; set; }
    public Guid? InvoiceId { get; set; }
    public string? InvoiceRef { get; set; }
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
    public string? CreatedBy { get; set; }
    public double? TotalPriceExcludingVAT { get; set; }
    public double? TotalPriceIncludingVAT { get; set; }
}
