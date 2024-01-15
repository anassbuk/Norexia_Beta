using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.UnitTypes.Commands.CreateUnitType;

public class CreateUnitTypeCommandHandler : IRequestHandler<CreateUnitTypeCommand, Guid>
{

    private readonly IApplicationDbContext _context;
    public CreateUnitTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateUnitTypeCommand request, CancellationToken cancellationToken)
    {
        UnitType unitType = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
        };

        var result = await _context.UnitTypes.AddAsync(unitType);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}
