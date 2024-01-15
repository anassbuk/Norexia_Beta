using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.ProviderEntities;

namespace Norexia.Core.Domain.PurchaseOrderEntities;
public class PurchaseOrder : BaseAuditableEntity
{
    public Guid ProviderId { get; set; }
    public string? Reference { get; set; }
    public virtual Provider? Provider { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? Status { get; set; }
    public string? Note { get; set; }
    public virtual ICollection<PurchaseOrderLine>? PurchaseOrderLines { get; set; }
}
