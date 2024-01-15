using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Domain.InvoiceEntities;
public class InvoiceLine : BaseAuditableEntity
{
    public Guid? SellingPriceId { get; set; }
    public virtual SellingPrice? SellingPrice { get; set; }
    public string? DeliveryRef { get; set; }
    public Guid? InvoiceId { get; set; }
    public virtual Invoice? Invoice { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product? Product { get; set; }
    public double? Price { get; set; }
    public int? VAT { get; set; }
    public int? Discount { get; set; }
    public int? Qty { get; set; }
    public int? ExpectedQty { get; set; }
}
