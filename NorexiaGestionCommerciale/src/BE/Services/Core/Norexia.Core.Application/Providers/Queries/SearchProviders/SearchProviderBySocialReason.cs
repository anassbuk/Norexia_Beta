using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Providers.Queries.GetProviders;

namespace Norexia.Core.Application.Providers.Queries.SearchProviders;

public record SearchProviderQueryBySocialReason(string reasonsocial) : IRequest<IEnumerable<ProvidersDto>>;
public class SearchProviderBySocialReason : IRequestHandler<SearchProviderQueryBySocialReason, IEnumerable<ProvidersDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchProviderBySocialReason(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<ProvidersDto>> Handle(SearchProviderQueryBySocialReason request, CancellationToken cancellationToken)
    {
        return await _context.Providers
        .AsNoTracking()
        .ProjectTo<ProvidersDto>(_mapper.ConfigurationProvider)
        .Where(t => t.SocialReason!.ToLower().Contains(request.reasonsocial.ToLower())).ToListAsync(cancellationToken);
    }
}
