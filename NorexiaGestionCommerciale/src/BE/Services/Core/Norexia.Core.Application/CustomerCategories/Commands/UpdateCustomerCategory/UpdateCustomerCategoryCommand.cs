using MediatR;

namespace Norexia.Core.Application.CustomerCategories.Commands.UpdateCustomerCategory;
public class UpdateCustomerCategoryCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
