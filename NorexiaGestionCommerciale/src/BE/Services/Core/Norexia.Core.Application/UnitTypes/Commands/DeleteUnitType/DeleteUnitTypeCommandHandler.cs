using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.UnitTypes.Commands.DeleteUnitType;

public class DeleteUnitTypeCommandHandler : IRequestHandler<DeleteUnitTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteUnitTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteUnitTypeCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var unitType = await _context.UnitTypes.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (unitType == null)
            {
                throw new NotFoundException($"Unit with id ({id}) not found! ");
            }

            unitType.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
public record DeleteUnitTypeCommand(IEnumerable<Guid> Ids) : IRequest;
