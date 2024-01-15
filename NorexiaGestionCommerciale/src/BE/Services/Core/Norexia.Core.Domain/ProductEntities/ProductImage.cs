using Norexia.Core.Domain.Common;

namespace Norexia.Core.Domain.ProductEntities;
public class ProductImage : BaseAuditableEntity
{
    public Guid ProductId { get; set; }
    public string? Path { get; set; }
}
