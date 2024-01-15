using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.ProductAvailabilities.Queries.GetProductAvailabilities;

public record GetProductAvailabilitiesQuery : IRequest<IEnumerable<ProductAvailabilityDto>>;
public class GetProductAvailabilitiesQueryHandler : IRequestHandler<GetProductAvailabilitiesQuery, IEnumerable<ProductAvailabilityDto>>
{
    private IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetProductAvailabilitiesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductAvailabilityDto>> Handle(GetProductAvailabilitiesQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductAvailabilities
                        .AsNoTracking()
                        .Where(c => c.IsDeleted == false)
                        .ProjectTo<ProductAvailabilityDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}
