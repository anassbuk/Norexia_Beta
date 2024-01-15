using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.ClassKeys.Queries.GetClasses;

public class ClassValueDto : IMapFrom<ClassValue>
{
    public Guid Id { get; set; }
    public string? Value { get; set; }
}
