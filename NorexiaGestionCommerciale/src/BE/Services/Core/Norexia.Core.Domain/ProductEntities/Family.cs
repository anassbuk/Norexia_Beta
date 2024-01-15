using Norexia.Core.Domain.Common;

namespace Norexia.Core.Domain.ProductEntities;
public class Family : BaseAuditableEntity
{
    public Guid? ParentFamilyId { get; set; }
    public virtual Family? ParentFamily { get; set; }
    public string? Designation { get; set; }
    public virtual ICollection<Family>? SubFamilies { get; set; }
}