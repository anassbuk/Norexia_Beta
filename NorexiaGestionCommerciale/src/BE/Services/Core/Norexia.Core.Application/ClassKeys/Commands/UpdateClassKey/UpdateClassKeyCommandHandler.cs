using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.ClassKeys.Commands.UpdateClassKey;
public class UpdateClassKeyCommandHandler : IRequestHandler<UpdateClassKeyCommand, Guid>
{
    private IApplicationDbContext _context;
    public UpdateClassKeyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateClassKeyCommand request, CancellationToken cancellationToken)
    {
        var classKey = await _context.ClassKeys.SingleOrDefaultAsync(f => f.Id == request.Id, cancellationToken: cancellationToken);

        if (classKey == null)
            throw new NotFoundException($"Class with id ({request.Id}) not found! ");

        classKey.Key = request.Key;

        await _context.SaveChangesAsync(cancellationToken);

        return classKey.Id;
    }
}
