using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Domain.QuotationEntities;

public class QuotationLine
{
    public Guid? SellingPriceId { get; set; }
    public virtual SellingPrice? SellingPrice { get; set; }
    public Guid? QuotationID { get; set; }
    public virtual Quotation? Quotation { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product? Product { get; set; }
    public double? Price { get; set; }
    public double? Margin { get; set; }
    public int? VAT { get; set; }
    public int? Discount { get; set; }
    public int? Qty { get; set; }
    public Guid Id { get; set; }
}
