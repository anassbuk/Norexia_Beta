using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.ClassKeys.Commands.CreateClassKey;

public class CreateClassKeyCommandHandler : IRequestHandler<CreateClassKeyCommand, Guid>
{

    private readonly IApplicationDbContext _context;
    public CreateClassKeyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateClassKeyCommand request, CancellationToken cancellationToken)
    {
        ClassKey key = new()
        {
            Id = Guid.NewGuid(),
            Key = request.Key,
        };

        var result = await _context.ClassKeys.AddAsync(key);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}
