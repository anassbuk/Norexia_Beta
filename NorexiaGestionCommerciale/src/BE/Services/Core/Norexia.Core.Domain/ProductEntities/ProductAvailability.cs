using Norexia.Core.Domain.Common;

namespace Norexia.Core.Domain.ProductEntities;
public class ProductAvailability : BaseAuditableEntity
{
    public string? Name { get; set; }
    public virtual ICollection<Product>? Products { get; set; }
}
