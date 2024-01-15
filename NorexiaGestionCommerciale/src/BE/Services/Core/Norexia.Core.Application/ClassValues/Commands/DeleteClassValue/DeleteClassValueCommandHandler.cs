using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.ClassValues.Commands.DeleteClassValue;

public class DeleteClassValueCommandHandler : IRequestHandler<DeleteClassValueCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteClassValueCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteClassValueCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var classValue = await _context.ClassValues.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (classValue == null)
            {
                throw new NotFoundException($"Class value with id ({id}) not found! ");
            }

            classValue.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
public record DeleteClassValueCommand(IEnumerable<Guid> Ids) : IRequest;
