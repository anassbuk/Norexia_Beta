using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;

namespace Norexia.Core.Domain.ProductEntities;

[Owned]
public class ClassificationInfo
{
    public Guid? FamilyId { get; set; }

    [ForeignKey(nameof(FamilyId))]
    public virtual Family? Family { get; set; }

}
