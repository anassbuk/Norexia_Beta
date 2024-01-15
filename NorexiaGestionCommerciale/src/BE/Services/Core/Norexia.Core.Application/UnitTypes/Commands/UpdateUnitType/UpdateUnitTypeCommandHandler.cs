using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.UnitTypes.Commands.UpdateUnitType;

public class UpdateUnitTypeCommandHandler : IRequestHandler<UpdateUnitTypeCommand, Guid>
{
    private IApplicationDbContext _context;
    public UpdateUnitTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateUnitTypeCommand request, CancellationToken cancellationToken)
    {
        var unitType = await _context.UnitTypes.SingleOrDefaultAsync(f => f.Id == request.Id, cancellationToken: cancellationToken);

        if (unitType == null)
            throw new NotFoundException($"Unit with id ({request.Id}) not found! ");

        unitType.Name = unitType.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return unitType.Id;
    }
}
