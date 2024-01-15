using Norexia.Core.Domain.Common;

namespace Norexia.Core.Domain.ProductEntities;
public class ProductNote : BaseAuditableEntity
{
    public Guid ProductId { get; set; }
    public string? Note { get; set; }
}
