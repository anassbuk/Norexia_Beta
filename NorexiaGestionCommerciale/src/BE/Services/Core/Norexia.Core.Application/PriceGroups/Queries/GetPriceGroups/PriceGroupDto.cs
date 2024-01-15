using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.PriceGroups.Queries.GetPriceGroups;
public class PriceGroupDto : IMapFrom<PriceGroup>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
