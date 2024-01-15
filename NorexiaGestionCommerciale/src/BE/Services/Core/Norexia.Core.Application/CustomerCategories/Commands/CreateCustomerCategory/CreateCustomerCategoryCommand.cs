using MediatR;

namespace Norexia.Core.Application.CustomerCategories.Commands.CreateCustomerCategory;
public class CreateCustomerCategoryCommand : IRequest<Guid>
{
    public string? Name { get; set; }
}
