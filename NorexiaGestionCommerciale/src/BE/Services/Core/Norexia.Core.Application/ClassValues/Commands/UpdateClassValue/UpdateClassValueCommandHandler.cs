using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.ClassValues.Commands.UpdateClassValue;

public class UpdateClassValueCommandHandler : IRequestHandler<UpdateClassValueCommand, Guid>
{
    private IApplicationDbContext _context;
    public UpdateClassValueCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateClassValueCommand request, CancellationToken cancellationToken)
    {
        var classValue = await _context.ClassValues.SingleOrDefaultAsync(f => f.Id == request.Id, cancellationToken: cancellationToken);

        if (classValue == null)
            throw new NotFoundException($"Class value with id ({request.Id}) not found! ");

        classValue.Value = request.Value;
        classValue.ProductClassKeyId = request.ClassId;

        await _context.SaveChangesAsync(cancellationToken);

        return classValue.Id;
    }
}
