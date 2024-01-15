using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.ClassKeys.Commands.DeleteClassKey;

public class DeleteClassKeyCommandHandler : IRequestHandler<DeleteClassKeyCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteClassKeyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteClassKeyCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var classKey = await _context.ClassKeys.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (classKey == null)
            {
                throw new NotFoundException($"Class with id ({id}) not found! ");
            }

            classKey.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record DeleteClassKeyCommand(IEnumerable<Guid> Ids) : IRequest;
