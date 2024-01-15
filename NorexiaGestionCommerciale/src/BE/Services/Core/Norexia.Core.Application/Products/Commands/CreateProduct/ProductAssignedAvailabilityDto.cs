using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.Products.Commands.CreateProduct;
public class ProductAssignedAvailabilityDto : IMapFrom<ProductAvailability>
{
    public Guid ProductAvailabilityId { get; set; }
}
