using MediatR;

using Norexia.Core.Application.Products.Queries.GetProduct;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.Common.Models;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.Products.Commands.CreateProduct;
public class CreateProductCommand : IRequest<Guid>
{
    public string? Reference { get; set; }
    public string? LongDesignation { get; set; }
    public string? ShortDesignation { get; set; }
    public string? Description { get; set; }
    public ProductType? Type { get; set; }
    public ProductAction? Action { get; set; }
    public string? Barcode { get; set; }
    public ClassificationInfo? ClassificationInfo { get; set; } = new();
    public PurchaseInfo? PurchaseInfo { get; set; } = new();
    public SellInfo? SellInfo { get; set; } = new();
    public UnitInfo? UnitInfo { get; set; } = new();
    public StorageSupplyInfo? StorageSupplyInfo { get; set; } = new();
    public ICollection<FileBase64>? ProductImages { get; set; }
    public ICollection<NoteDto>? ProductNotes { get; set; }
    public ICollection<ProductClassDto>? ProductClassifications { get; set; }
    public ICollection<ProductUnitDto>? ProductUnits { get; set; }
    public ICollection<SellingPriceDto>? SellingPrices { get; set; }
    public ICollection<ProductAssignedAvailabilityDto>? ProductAvailabilities { get; set; }
}
