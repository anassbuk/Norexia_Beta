using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.PriceGroups.Commands.UpdatePriceGroup;
public class UpdatePriceGroupCommandHandler : IRequestHandler<UpdatePriceGroupCommand, Guid>
{
    private IApplicationDbContext _context;
    public UpdatePriceGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdatePriceGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _context.PriceGroups.SingleOrDefaultAsync(f => f.Id == request.Id, cancellationToken: cancellationToken);

        if (group == null)
            throw new NotFoundException($"Class with id ({request.Id}) not found! ");

        group.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return group.Id;
    }
}
