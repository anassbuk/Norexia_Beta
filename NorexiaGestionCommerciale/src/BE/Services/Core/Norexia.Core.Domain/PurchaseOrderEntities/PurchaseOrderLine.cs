using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Domain.PurchaseOrderEntities;
public class PurchaseOrderLine : BaseAuditableEntity
{
    public Guid PurchaseOrderId { get; set; }
    public virtual PurchaseOrder? PurchaseOrder { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product? Product { get; set; }
    public double? Price { get; set; }
    public int? VAT { get; set; }
    public int? Qty { get; set; }
}
