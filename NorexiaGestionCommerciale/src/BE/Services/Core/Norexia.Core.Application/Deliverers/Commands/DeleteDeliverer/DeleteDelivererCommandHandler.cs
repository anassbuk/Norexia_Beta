using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Deliverers.Commands.DeleteDeliverer;
public class DeleteDelivererCommandHandler : IRequestHandler<DeleteDelivererCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteDelivererCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteDelivererCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var deliverer = await _context.Deliverers.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (deliverer == null)
            {
                throw new NotFoundException($"Deliverer with id ({id}) not found! ");
            }

            deliverer.IsDeleted = true;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
public record DeleteDelivererCommand(IEnumerable<Guid> Ids) : IRequest;
