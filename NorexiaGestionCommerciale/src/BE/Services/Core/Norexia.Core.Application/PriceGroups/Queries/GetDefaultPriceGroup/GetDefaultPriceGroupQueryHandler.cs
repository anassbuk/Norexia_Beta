using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.PriceGroups.Queries.GetDefaultPriceGroup;

public record GetDefaultPriceGroupQuery : IRequest<Guid>;
public class GetDefaultPriceGroupQueryHandler : IRequestHandler<GetDefaultPriceGroupQuery, Guid>
{
    private IApplicationDbContext _context;
    public GetDefaultPriceGroupQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(GetDefaultPriceGroupQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.PriceGroups.AnyAsync(c => c.Name == "__default", cancellationToken))
        {
            PriceGroup group = new()
            {
                Id = Guid.NewGuid(),
                Name = "__default"
            };

            var result = await _context.PriceGroups.AddAsync(group);
            await _context.SaveChangesAsync(cancellationToken);
            return result.Entity.Id;
        }
        else
            return _context.PriceGroups
                            .AsNoTracking()
                            .FirstAsync(c => c.Name == "__default", cancellationToken).Result.Id;
    }
}
