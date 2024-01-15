using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.PriceGroups.Commands.CreatePriceGroup;
public class CreatePriceGroupCommandHandler : IRequestHandler<CreatePriceGroupCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreatePriceGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreatePriceGroupCommand request, CancellationToken cancellationToken)
    {
        PriceGroup group = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        var result = await _context.PriceGroups.AddAsync(group, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}
