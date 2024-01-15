using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Application.Families.Queries.GetFamilies;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.Products.Queries.GetProducts;
public class ProductDto : IMapFrom<Product>
{
    public Guid Id { get; set; }
    public string? ShortDesignation { get; set; }
    public string? Reference { get; set; }
    public DateTime Created { get; set; }
    public ProductType? Type { get; set; }
    public FamilyDto? Family { get; set; }
    public FamilyDto? SubFamily { get; set; }
    public double? DefaultSalePrice_TaxeIncluded { get; set; }
    public double? DefaultSalePrice_TaxeExcluded { get; set; }
    public StorageSupplyInfo? StorageSupplyInfo { get; set; }
    public ProductAction? Action { get; set; }
    public bool Active { get; set; }
    public bool MultiPrice { get; set; }
}
