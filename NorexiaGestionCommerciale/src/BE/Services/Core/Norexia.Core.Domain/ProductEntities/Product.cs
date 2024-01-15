using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Domain.ProductEntities;
public class Product : BaseAuditableEntity
{
    public string? Reference { get; set; }
    public string? LongDesignation { get; set; }
    public string? ShortDesignation { get; set; }
    public string? Description { get; set; }
    public ProductType? Type { get; set; }
    public ProductAction? Action { get; set; }
    public string? Barcode { get; set; }
    public required ClassificationInfo ClassificationInfo { get; set; }
    public required PurchaseInfo PurchaseInfo { get; set; }
    public required SellInfo SellInfo { get; set; }
    public required UnitInfo UnitInfo { get; set; }
    public required StorageSupplyInfo StorageSupplyInfo { get; set; }
    public bool Active { get; set; } = false;
    public virtual ICollection<ProductImage>? Images { get; set; }
    public virtual ICollection<ProductNote>? Notes { get; set; }
    public virtual ICollection<ClassValue>? ProductClassValues { get; set; }
    public virtual ICollection<UnitMeasurement>? ProductUnitMeasurements { get; set; }
    public virtual ICollection<SellingPrice>? SellingPrices { get; set; }
    public virtual ICollection<ProductAvailability>? ProductAvailabilities { get; set; }
}
