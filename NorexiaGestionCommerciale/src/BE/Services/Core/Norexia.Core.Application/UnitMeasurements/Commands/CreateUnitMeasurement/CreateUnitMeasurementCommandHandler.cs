using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.UnitMeasurements.Commands.CreateUnitMeasurement;

public class CreateUnitMeasurementCommandHandler : IRequestHandler<CreateUnitMeasurementCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateUnitMeasurementCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateUnitMeasurementCommand request, CancellationToken cancellationToken)
    {
        UnitMeasurement measurement = new()
        {
            Id = Guid.NewGuid(),
            UnitTypeId = request.UnitId,
            Name = request.Name,
        };

        var result = await _context.UnitMeasurements.AddAsync(measurement);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}
