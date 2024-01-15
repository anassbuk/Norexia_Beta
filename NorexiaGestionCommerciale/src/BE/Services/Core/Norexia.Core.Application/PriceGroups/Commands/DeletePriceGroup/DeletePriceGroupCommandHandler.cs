using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.PriceGroups.Commands.DeletePriceGroup;

public class DeletePriceGroupCommandHandler : IRequestHandler<DeletePriceGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePriceGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeletePriceGroupCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var group = await _context.PriceGroups.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (group == null)
            {
                throw new NotFoundException($"Class with id ({id}) not found! ");
            }

            group.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record DeletePriceGroupCommand(IEnumerable<Guid> Ids) : IRequest;
