using MediatR;

namespace Norexia.Core.Application.UnitMeasurements.Commands.UpdateUnitMeasurement;

public class UpdateUnitMeasurementCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public Guid UnitId { get; set; }
    public string? Name { get; set; }
}
