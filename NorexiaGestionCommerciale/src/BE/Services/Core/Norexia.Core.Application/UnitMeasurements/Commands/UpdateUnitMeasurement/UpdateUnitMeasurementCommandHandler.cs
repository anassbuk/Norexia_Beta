using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.UnitMeasurements.Commands.UpdateUnitMeasurement;

public class UpdateUnitMeasurementCommandHandler : IRequestHandler<UpdateUnitMeasurementCommand, Guid>
{
    private IApplicationDbContext _context;
    public UpdateUnitMeasurementCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateUnitMeasurementCommand request, CancellationToken cancellationToken)
    {
        var unitMeasurement = await _context.UnitMeasurements.SingleOrDefaultAsync(f => f.Id == request.Id, cancellationToken: cancellationToken);

        if (unitMeasurement == null)
            throw new NotFoundException($"Unit measurement with id ({request.Id}) not found! ");

        unitMeasurement.Name = request.Name;
        unitMeasurement.UnitTypeId = request.UnitId;

        await _context.SaveChangesAsync(cancellationToken);

        return unitMeasurement.Id;
    }
}
