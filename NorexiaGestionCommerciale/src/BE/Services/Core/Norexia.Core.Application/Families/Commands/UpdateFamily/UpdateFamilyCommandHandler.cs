using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Families.Commands.UpdateFamily;
public class UpdateFamilyCommandHandler : IRequestHandler<UpdateFamilyCommand, Guid>
{

    private readonly IApplicationDbContext _context;
    public UpdateFamilyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> Handle(UpdateFamilyCommand request, CancellationToken cancellationToken)
    {
        if (request.ParentFamilyId != null && !_context.Families.Any(f => f.Id == request.ParentFamilyId))
            throw new NotFoundException($"Family with id ({request.ParentFamilyId}) not found! ");

        var family = await _context.Families.SingleOrDefaultAsync(f => f.Id == request.Id, cancellationToken: cancellationToken);

        if (family == null)
            throw new NotFoundException($"Family with id ({request.Id}) not found! ");

        family.Designation = request.Designation;
        family.ParentFamilyId = request.ParentFamilyId;

        await _context.SaveChangesAsync(cancellationToken);

        return family.Id;
    }
}