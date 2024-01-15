using Microsoft.EntityFrameworkCore;

namespace Norexia.Core.Domain.ProductEntities;
[Owned]
public class StorageSupplyInfo
{
    public int? Quantity { get; set; }
    public int? MaximumStock { get; set; }
    public int? MinimumStock { get; set; }
    public int? SafetyStock { get; set; }
    public int? BatchSize { get; set; }
    public int? RetentionPeriod { get; set; }
}
