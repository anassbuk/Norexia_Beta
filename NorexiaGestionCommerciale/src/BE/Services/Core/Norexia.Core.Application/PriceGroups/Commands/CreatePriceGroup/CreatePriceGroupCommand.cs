using MediatR;

namespace Norexia.Core.Application.PriceGroups.Commands.CreatePriceGroup;
public class CreatePriceGroupCommand : IRequest<Guid>
{
    public string? Name { get; set; }
}