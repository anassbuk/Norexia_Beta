using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Deliverers.Commands.ActivateDeliverer;
public class ActivateDelivererCommandHandler : IRequestHandler<ActivateDelivererCommand>
{
    private readonly IApplicationDbContext _context;
    public ActivateDelivererCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(ActivateDelivererCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var deliverer = await _context.Deliverers.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (deliverer == null)
            {
                throw new NotFoundException($"Deliverer with id ({id}) not found! ");
            }

            deliverer.Active = !deliverer.Active;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
public record ActivateDelivererCommand(IEnumerable<Guid> Ids) : IRequest;
