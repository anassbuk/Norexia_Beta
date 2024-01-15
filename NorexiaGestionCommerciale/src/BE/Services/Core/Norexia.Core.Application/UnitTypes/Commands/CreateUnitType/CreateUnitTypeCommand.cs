using MediatR;

namespace Norexia.Core.Application.UnitTypes.Commands.CreateUnitType;

public class CreateUnitTypeCommand : IRequest<Guid>
{
    public string? Name { get; set; }
}
