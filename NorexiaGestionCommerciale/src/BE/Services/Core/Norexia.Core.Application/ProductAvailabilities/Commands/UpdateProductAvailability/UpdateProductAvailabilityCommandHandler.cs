using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.ProductAvailabilities.Commands.UpdateProductAvailability;

public class UpdateProductAvailabilityCommandHandler : IRequestHandler<UpdateProductAvailabilityCommand, Guid>
{
    private IApplicationDbContext _context;
    public UpdateProductAvailabilityCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateProductAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var availability = await _context.ProductAvailabilities.SingleOrDefaultAsync(f => f.Id == request.Id, cancellationToken: cancellationToken);

        if (availability == null)
            throw new NotFoundException($"Product availability with id ({request.Id}) not found! ");

        availability.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return availability.Id;
    }
}
