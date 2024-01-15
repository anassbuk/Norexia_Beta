using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.ProductEntities;

using System.Linq.Expressions;

namespace Norexia.Core.Application.Families.Queries.GetFamilies;

public record GetFamiliesQuery : IRequest<IEnumerable<FamilyDto>>;
public class GetFamiliesQueryHandler : IRequestHandler<GetFamiliesQuery, IEnumerable<FamilyDto>>
{
    private IApplicationDbContext _context;
    public GetFamiliesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FamilyDto>> Handle(GetFamiliesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Families
                        .AsNoTracking()
                        .Where(f => f.ParentFamilyId == null && f.IsDeleted == false)
                        .Select(GetFamilyProjection(2, 0));

        return await query.ToListAsync(cancellationToken);
    }

    public static Expression<Func<Family, FamilyDto>> GetFamilyProjection(int maxDepth, int currentDepth = 0)
    {
        currentDepth++;

        Expression<Func<Family, FamilyDto>> result = family => new FamilyDto()
        {
            FamilyId = family.Id,
            Designation = family.Designation,
            SubFamilies = currentDepth == maxDepth
            ? new List<FamilyDto>()
            : family.SubFamilies!.AsQueryable()
                .Where(f => f.IsDeleted == false)
                .Select(GetFamilyProjection(maxDepth, currentDepth)).ToList()
        };

        return result;
    }
}
