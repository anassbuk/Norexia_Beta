using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Domain.SaleOrderEntities;
public class SaleOrderLine : BaseAuditableEntity
{
    public Guid? SellingPriceId { get; set; }
    public virtual SellingPrice? SellingPrice { get; set; }
    public Guid? SaleOrderId { get; set; }
    public virtual SaleOrder? SaleOrder { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product? Product { get; set; }
    public double? Price { get; set; }
    public double? Margin { get; set; }
    public int? VAT { get; set; }
    public int? Discount { get; set; }
    public int? Qty { get; set; }
}
