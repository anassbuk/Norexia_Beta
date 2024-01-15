using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.UnitTypes.Queries.GetUnits;

public record GetUnitsQuery : IRequest<IEnumerable<UnitDto>>;
public class GetUnitsQueryHandler : IRequestHandler<GetUnitsQuery, IEnumerable<UnitDto>>
{
    private IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetUnitsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UnitDto>> Handle(GetUnitsQuery request, CancellationToken cancellationToken)
    {
        return await _context.UnitTypes
                        .AsNoTracking()
                        .Where(c => c.IsDeleted == false)
                        .Include(c => c.Measurements!.Where(v => v.IsDeleted == false))
                        .ProjectTo<UnitDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}
