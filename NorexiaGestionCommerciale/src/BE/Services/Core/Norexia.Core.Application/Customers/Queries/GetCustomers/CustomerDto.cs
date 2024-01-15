using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Application.CustomerCategories.Queries.GetCustomerCategories;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.CustomerEntities;

namespace Norexia.Core.Application.Customers.Queries.GetCustomers;
public class CustomerDto : IMapFrom<Customer>
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public string? SocialReason { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public CustomerCategoryDto? CustomerCategory { get; set; }
    public ClientType? ClientType { get; set; }
    public string? Tel { get; set; }
    public bool Active { get; set; }
}
