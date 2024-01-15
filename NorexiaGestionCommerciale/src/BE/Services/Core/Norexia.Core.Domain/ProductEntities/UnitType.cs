using Norexia.Core.Domain.Common;

namespace Norexia.Core.Domain.ProductEntities;

public class UnitType : BaseAuditableEntity
{
    public string? Name { get; set; }
    public virtual ICollection<UnitMeasurement>? Measurements { get; set; }
}
