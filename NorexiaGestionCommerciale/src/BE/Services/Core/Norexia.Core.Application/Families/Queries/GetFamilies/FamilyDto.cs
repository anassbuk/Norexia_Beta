using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.Families.Queries.GetFamilies;

public class FamilyDto : IMapFrom<Family>
{
    public Guid FamilyId { get; set; }
    public string? Designation { get; set; }
    public ICollection<FamilyDto>? SubFamilies { get; set; }
}
