using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.Families.Commands.CreateFamily;

public class CreateFamilyCommandHandler : IRequestHandler<CreateFamilyCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateFamilyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateFamilyCommand request, CancellationToken cancellationToken)
    {
        if (request.ParentFamilyId != null && !_context.Families.Any(f => f.Id == request.ParentFamilyId))
            throw new NotFoundException($"Family with id ({request.ParentFamilyId}) not found! ");

        Family family = new()
        {
            Id = Guid.NewGuid(),
            ParentFamilyId = request.ParentFamilyId,
            Designation = request.Designation,
        };

        var result = await _context.Families.AddAsync(family);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}
