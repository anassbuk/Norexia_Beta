using Norexia.Core.Domain.Common;

namespace Norexia.Core.Domain.ProductEntities;
public class SellingPrice : BaseAuditableEntity
{
    public Guid? PriceGroupId { get; set; }
    public virtual PriceGroup? PriceGroup { get; set; }
    public double? Price { get; set; }
    public double? Margin { get; set; }
    public int? VAT { get; set; }
    public int? Discount { get; set; }
    public Guid ProductId { get; set; }
}