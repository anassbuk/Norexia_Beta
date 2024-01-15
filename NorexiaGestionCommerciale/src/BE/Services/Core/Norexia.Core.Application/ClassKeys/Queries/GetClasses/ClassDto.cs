using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.ClassKeys.Queries.GetClasses;

public class ClassDto : IMapFrom<ClassKey>
{
    public Guid Id { get; set; }
    public string? Key { get; set; }
    public ICollection<ClassValueDto>? Values { get; set; }
}
