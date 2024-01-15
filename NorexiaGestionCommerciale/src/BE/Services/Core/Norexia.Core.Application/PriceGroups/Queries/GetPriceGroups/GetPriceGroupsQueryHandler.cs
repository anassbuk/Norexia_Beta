using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.PriceGroups.Queries.GetPriceGroups;

public record GetPriceGroupsQuery : IRequest<IEnumerable<PriceGroupDto>>;
public class GetPriceGroupsQueryHandler : IRequestHandler<GetPriceGroupsQuery, IEnumerable<PriceGroupDto>>
{
    private IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetPriceGroupsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PriceGroupDto>> Handle(GetPriceGroupsQuery request, CancellationToken cancellationToken)
    {
        return await _context.PriceGroups
                        .AsNoTracking()
                        .Where(c => c.IsDeleted == false && c.Name != "__default")
                        .ProjectTo<PriceGroupDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}
