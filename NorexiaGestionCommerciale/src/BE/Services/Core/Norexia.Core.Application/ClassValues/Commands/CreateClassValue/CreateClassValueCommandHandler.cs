using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.ClassValues.Commands.CreateClassValue;
public class CreateClassValueCommandHandler : IRequestHandler<CreateClassValueCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateClassValueCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateClassValueCommand request, CancellationToken cancellationToken)
    {
        ClassValue classValue = new()
        {
            Id = Guid.NewGuid(),
            ProductClassKeyId = request.ClassId,
            Value = request.Value,
        };

        var result = await _context.ClassValues.AddAsync(classValue);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}
