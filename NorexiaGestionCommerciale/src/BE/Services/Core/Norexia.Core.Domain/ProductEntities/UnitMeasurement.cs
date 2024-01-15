using Norexia.Core.Domain.Common;

namespace Norexia.Core.Domain.ProductEntities;
public class UnitMeasurement : BaseAuditableEntity
{
    public string? Name { get; set; }
    public Guid? UnitTypeId { get; set; }
    public virtual UnitType? UnitType { get; set; }
    public virtual ICollection<Product>? Products { get; set; }
}
