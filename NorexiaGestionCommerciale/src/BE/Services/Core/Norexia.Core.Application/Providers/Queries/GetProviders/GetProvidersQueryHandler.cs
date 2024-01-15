using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.Providers.Queries.GetProviders;

public record GetProvidersQuery : IRequest<IEnumerable<ProvidersDto>>;
public class GetProvidersQueryHandler : IRequestHandler<GetProvidersQuery, IEnumerable<ProvidersDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetProvidersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<ProvidersDto>> Handle(GetProvidersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Providers
                        .AsNoTracking()
                        .Where(c => !c.IsDeleted)
                        .Include(c => c.ProviderCategory)
                        .ProjectTo<ProvidersDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}
