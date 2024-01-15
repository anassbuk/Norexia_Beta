using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Domain.ProviderInvoiceEntities;
public class ProviderInvoiceLine : BaseAuditableEntity
{
    public Guid? ProviderInvoiceId { get; set; }
    public virtual ProviderInvoice? ProviderInvoice { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product? Product { get; set; }
    public double? Price { get; set; }
    public int? VAT { get; set; }
    public int? Discount { get; set; }
    public int? Qty { get; set; }
    public int? ExpectedQty { get; set; }
}
