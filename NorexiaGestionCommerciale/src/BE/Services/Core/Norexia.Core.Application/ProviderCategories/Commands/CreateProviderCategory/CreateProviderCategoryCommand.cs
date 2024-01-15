using MediatR;

namespace Norexia.Core.Application.ProviderCategories.Commands.CreateProviderCategory;
public class CreateProviderCategoryCommand : IRequest<Guid>
{
    public string? Name { get; set; }
}
