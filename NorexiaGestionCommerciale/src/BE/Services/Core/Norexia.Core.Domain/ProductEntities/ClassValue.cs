using Norexia.Core.Domain.Common;

namespace Norexia.Core.Domain.ProductEntities;
public class ClassValue : BaseAuditableEntity
{
    public string? Value { get; set; }
    public Guid? ProductClassKeyId { get; set; }
    public virtual ClassKey? ProductClassKey { get; set; }
    public virtual ICollection<Product>? Products { get; set; }
}
