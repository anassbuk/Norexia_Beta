using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.PurchaseOrderEntities;

namespace Norexia.Core.Application.PurchaseOrders.Queries.GetPurchaseOrderLines;
public class PurchaseOrderLineDto : IMapFrom<PurchaseOrderLine>
{
    public Guid Id { get; set; }
    public Guid? ProductId { get; set; }
    public string? Reference { get; set; }
    public string? ShortDesignation { get; set; }
    public double? Price { get; set; }
    public int? VAT { get; set; }
    public int? Qty { get; set; }
    public double? TotalPriceExcludingTax { get; set; }
    public double? TotalVATPrice { get; set; }
    public double? TotalPriceIncludingTax { get; set; }
}
