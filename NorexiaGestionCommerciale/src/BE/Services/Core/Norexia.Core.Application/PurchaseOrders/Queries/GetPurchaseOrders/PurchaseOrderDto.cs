using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.PurchaseOrderEntities;

namespace Norexia.Core.Application.PurchaseOrders.Queries.GetPurchaseOrders;
public class PurchaseOrderDto : IMapFrom<PurchaseOrder>
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? ProviderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? Status { get; set; }
    public double? PriceExcludingTax { get; set; }
    public double? TaxPrice { get; set; }
    public double? PriceIncludingTax { get; set; }
    public double? NetPrice { get; set; }
    public string? Note { get; set; }
}
