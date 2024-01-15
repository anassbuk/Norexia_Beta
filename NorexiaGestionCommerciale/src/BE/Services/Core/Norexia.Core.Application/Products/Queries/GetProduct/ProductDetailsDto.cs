using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Application.Products.Commands.CreateProduct;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.Common.Models;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.Products.Queries.GetProduct;
public class ProductDetailsDto : IMapFrom<Product>
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public string? LongDesignation { get; set; }
    public string? ShortDesignation { get; set; }
    public string? Description { get; set; }
    public ProductType? Type { get; set; }
    public ProductAction? Action { get; set; }
    public string? Barcode { get; set; }
    public ClassificationInfo? ClassificationInfo { get; set; } = new();
    public PurchaseInfo? PurchaseInfo { get; set; }
    public SellInfo? SellInfo { get; set; }
    public UnitInfo? UnitInfo { get; set; }
    public StorageSupplyInfo? StorageSupplyInfo { get; set; }
    public bool Active { get; set; } = false;
    public ICollection<FileBase64>? ProductImages { get; set; }
    public ICollection<NoteDto>? ProductNotes { get; set; }
    public ICollection<ProductClassDto>? ProductClassifications { get; set; }
    public ICollection<ProductUnitDto>? ProductUnits { get; set; }
    public ICollection<SellingPriceDto>? SellingPrices { get; set; }
    public ICollection<ProductAssignedAvailabilityDto>? ProductAvailabilities { get; set; }
}
