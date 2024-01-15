using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Application.ProviderCategories.Queries.GetProviderCategories;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.ProviderEntities;

namespace Norexia.Core.Application.Providers.Queries.GetProviders;
public class ProvidersDto : IMapFrom<Provider>
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public string? SocialReason { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public ProviderCategoryDto? ProviderCategory { get; set; }
    public ProviderType? ProviderType { get; set; }
    public string? Tel { get; set; }
    public bool Active { get; set; }
}
