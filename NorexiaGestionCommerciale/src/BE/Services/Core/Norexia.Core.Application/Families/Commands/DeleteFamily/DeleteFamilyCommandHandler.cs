using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Families.Commands.DeleteFamily;
public class DeleteFamilyCommandHandler : IRequestHandler<DeleteFamilyCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteFamilyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteFamilyCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var family = await _context.Families.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (family == null)
            {
                throw new NotFoundException($"Family with id ({id}) not found! ");
            }

            family.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}


public record DeleteFamilyCommand(IEnumerable<Guid> Ids) : IRequest;