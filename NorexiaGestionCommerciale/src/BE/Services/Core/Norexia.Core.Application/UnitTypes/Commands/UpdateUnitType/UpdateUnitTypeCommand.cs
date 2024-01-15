using MediatR;

namespace Norexia.Core.Application.UnitTypes.Commands.UpdateUnitType;

public class UpdateUnitTypeCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
