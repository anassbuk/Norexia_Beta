using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.ProductAvailabilities.Commands.DeleteProductAvailability;
public class DeleteProductAvailabilityCommandHandler : IRequestHandler<DeleteProductAvailabilityCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductAvailabilityCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProductAvailabilityCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var availability = await _context.ProductAvailabilities.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (availability == null)
            {
                throw new NotFoundException($"Product availability with id ({id}) not found! ");
            }

            availability.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record DeleteProductAvailabilityCommand(IEnumerable<Guid> Ids) : IRequest;
