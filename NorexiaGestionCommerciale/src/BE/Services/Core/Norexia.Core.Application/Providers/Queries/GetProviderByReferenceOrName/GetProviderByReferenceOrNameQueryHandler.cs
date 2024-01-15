using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Providers.Queries.GetProvider;

namespace Norexia.Core.Application.Providers.Queries.GetProviderByReferenceOrName;
public record GetProviderByReferenceOrNameQuery(string term) : IRequest<ProviderDetailsDto?>;
public class GetProviderByReferenceOrNameQueryHandler : IRequestHandler<GetProviderByReferenceOrNameQuery, ProviderDetailsDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetProviderByReferenceOrNameQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProviderDetailsDto?> Handle(GetProviderByReferenceOrNameQuery request, CancellationToken cancellationToken)
    {
        return await _context.Providers
             .AsNoTracking()
             .ProjectTo<ProviderDetailsDto>(_mapper.ConfigurationProvider)
             .FirstOrDefaultAsync(t => t.Reference!.ToLower() == request.term.ToLower() ||
                                        (t.SocialReason != null && t.SocialReason!.ToLower() == request.term.ToLower()) ||
                                          t.LastName!.ToLower() == request.term.ToLower(), cancellationToken);
    }
}
