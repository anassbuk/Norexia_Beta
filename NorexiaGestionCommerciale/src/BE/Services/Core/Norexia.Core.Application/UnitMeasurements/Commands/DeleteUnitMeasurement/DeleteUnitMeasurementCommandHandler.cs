using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.UnitMeasurements.Commands.DeleteUnitMeasurement;

public class DeleteUnitMeasurementCommandHandler : IRequestHandler<DeleteUnitMeasurementCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteUnitMeasurementCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteUnitMeasurementCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var unitMeasurement = await _context.UnitMeasurements.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (unitMeasurement == null)
            {
                throw new NotFoundException($"Class value with id ({id}) not found! ");
            }

            unitMeasurement.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
public record DeleteUnitMeasurementCommand(IEnumerable<Guid> Ids) : IRequest;
