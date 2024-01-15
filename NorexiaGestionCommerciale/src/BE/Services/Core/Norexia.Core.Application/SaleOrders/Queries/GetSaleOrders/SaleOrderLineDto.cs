using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Application.Products.Commands.CreateProduct;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.SaleOrders.Queries.GetSaleOrders;
public class SaleOrderLineDto : IMapFrom<SaleOrderLine>
{
    public Guid Id { get; set; }
    public Guid? ProductId { get; set; }
    public Guid? SellingPriceId { get; set; }
    public string? Reference { get; set; }
    public string? ShortDesignation { get; set; }
    public double? Price { get; set; }
    public double? Margin { get; set; }
    public int? VAT { get; set; }
    public int? Discount { get; set; }
    public int? Qty { get; set; }
    public double? TotalPriceExcludingTax { get; set; }
    public double? TotalVATPrice { get; set; }
    public double? TotalPriceIncludingTax { get; set; }
    public ICollection<SellingPriceDto>? SellingPrices { get; set; }
}