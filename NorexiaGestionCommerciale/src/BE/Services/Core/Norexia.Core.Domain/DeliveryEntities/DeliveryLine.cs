using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Domain.DeliveryEntities;
public class DeliveryLine : BaseAuditableEntity
{
    public Guid? SellingPriceId { get; set; }
    public virtual SellingPrice? SellingPrice { get; set; }
    public Guid? ProductId { get; set; }
    public virtual Product? Product { get; set; }
    public Guid? DeliveryId { get; set; }
    public virtual Delivery? Delivery { get; set; }
    public int? Qty { get; set; }
    public int? ExpectedQty { get; set; }
    public double? UnitPrice { get; set; }
    public int? Discount { get; set; }
    public int? VAT { get; set; }
}
