using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Product;
public class ProductCommand
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public string? LongDesignation { get; set; }
    public string? ShortDesignation { get; set; }
    public string? Description { get; set; }
    public ProductType? Type { get; set; }
    public ProductAction? Action { get; set; }
    public string? Barcode { get; set; }
    public ClassificationInfo? ClassificationInfo { get; set; }
    public PurchaseInfo? PurchaseInfo { get; set; }
    public SellInfo? SellInfo { get; set; }
    public UnitInfo? UnitInfo { get; set; }
    public StorageSupplyInfo? StorageSupplyInfo { get; set; }
    public ICollection<FileBase64>? ProductImages { get; set; }
    public ICollection<NoteDto>? ProductNotes { get; set; }
    public ICollection<ProductClassDto>? ProductClassifications { get; set; }
    public ICollection<ProductUnitDto>? ProductUnits { get; set; }
    public ICollection<SellingPriceDto>? SellingPrices { get; set; }
    public ICollection<ProductAssignedAvailabilityDto>? ProductAvailabilities { get; set; }
}
