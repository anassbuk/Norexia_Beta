using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.ProductAvailabilities.Queries.GetProductAvailabilities;

public class ProductAvailabilityDto : IMapFrom<ProductAvailability>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
