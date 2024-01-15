using MediatR;

namespace Norexia.Core.Application.PriceGroups.Commands.UpdatePriceGroup;
public class UpdatePriceGroupCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
