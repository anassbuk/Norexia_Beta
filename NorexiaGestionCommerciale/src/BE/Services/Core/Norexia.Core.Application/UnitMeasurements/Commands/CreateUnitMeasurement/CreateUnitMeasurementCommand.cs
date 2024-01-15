using MediatR;

namespace Norexia.Core.Application.UnitMeasurements.Commands.CreateUnitMeasurement;

public class CreateUnitMeasurementCommand : IRequest<Guid>
{
    public Guid UnitId { get; set; }
    public string? Name { get; set; }
}
