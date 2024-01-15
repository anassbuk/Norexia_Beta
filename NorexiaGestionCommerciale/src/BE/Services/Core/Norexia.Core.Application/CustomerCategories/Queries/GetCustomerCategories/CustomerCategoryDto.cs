using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.CustomerEntities;

namespace Norexia.Core.Application.CustomerCategories.Queries.GetCustomerCategories;
public class CustomerCategoryDto : IMapFrom<CustomerCategory>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
