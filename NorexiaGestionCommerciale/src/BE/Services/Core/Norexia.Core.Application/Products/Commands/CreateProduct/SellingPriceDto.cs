using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.Products.Commands.CreateProduct;
public class SellingPriceDto : IMapFrom<SellingPrice>
{
    public Guid? Id { get; set; }
    public Guid? PriceGroupId { get; set; }
    public double? Price { get; set; }
    public double? Margin { get; set; }
    public int? VAT { get; set; }
    public int? Discount { get; set; }
}
