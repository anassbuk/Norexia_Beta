using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.Providers.Queries.GetProvider;
public record GetProviderQuery(Guid Id) : IRequest<ProviderDetailsDto?>;
public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, ProviderDetailsDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProviderQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ProviderDetailsDto?> Handle(GetProviderQuery request, CancellationToken cancellationToken)
    {
        return await _context.Providers
              .Include(p => p.ProviderAddresses)
              .AsNoTracking()
              .ProjectTo<ProviderDetailsDto>(_mapper.ConfigurationProvider)
              .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
    }
}
