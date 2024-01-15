using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.CustomerEntities;
using Norexia.Core.Domain.InvoiceEntities;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Domain.DeliveryEntities;
public class Delivery : BaseAuditableEntity
{
    public string? Reference { get; set; }
    public Guid? CustomerId { get; set; }
    public virtual Customer? Customer { get; set; }
    public Guid? DelivererId { get; set; }
    public virtual Deliverer? Deliverer { get; set; }
    public Guid? SaleOrderId { get; set; }
    public virtual SaleOrder? SaleOrder { get; set; }
    public Guid? InvoiceId { get; set; }
    public virtual Invoice? Invoice { get; set; }
    public DeliveryOrigin DeliveryOrigin { get; set; }
    public DateTime EntryDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime? PlannedDate { get; set; }
    public DateTime DeliveryTime { get; set; }
    public DeliveryMode? DeliveryMode { get; set; }
    public string? Status { get; set; }
    public string? Situation { get; set; }
    public StockRecordType? Type { get; set; }
    public string? Note { get; set; }
    public virtual ICollection<DeliveryLine>? DeliveryLines { get; set; }
}
