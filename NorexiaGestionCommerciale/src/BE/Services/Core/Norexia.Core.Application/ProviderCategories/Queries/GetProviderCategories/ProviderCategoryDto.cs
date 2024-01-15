using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.ProviderEntities;

namespace Norexia.Core.Application.ProviderCategories.Queries.GetProviderCategories;
public class ProviderCategoryDto : IMapFrom<ProviderCategory>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
