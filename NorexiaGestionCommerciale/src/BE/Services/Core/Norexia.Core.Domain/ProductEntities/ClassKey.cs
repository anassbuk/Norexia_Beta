using Norexia.Core.Domain.Common;

namespace Norexia.Core.Domain.ProductEntities;
public class ClassKey : BaseAuditableEntity
{
    public string? Key { get; set; }
    public virtual ICollection<ClassValue>? Values { get; set; }
}