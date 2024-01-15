using MediatR;

namespace Norexia.Core.Application.ProviderCategories.Commands.UpdateProviderCategory;
public class UpdateProviderCategoryCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
