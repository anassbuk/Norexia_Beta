using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Deliveries.Commands.DeleteDelivery;
public class DeleteDeliveryCommandHandler : IRequestHandler<DeleteDeliveryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteDeliveryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDeliveryCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var delivery = await _context.Deliveries.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (delivery == null)
            {
                throw new NotFoundException($"Delivery with id ({id}) not found! ");
            }

            delivery.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record DeleteDeliveryCommand(IEnumerable<Guid> Ids) : IRequest;
