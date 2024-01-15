using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Deliverers.Queries.GetDeliverers;

namespace Norexia.Core.Application.Deliverers.Queries.GetProviderByReferenceOrName;
public record GetDelivererByReferenceOrNameQuery(string term) : IRequest<DelivererDto?>;
public class GetDelivererByReferenceOrNameQueryHandler : IRequestHandler<GetDelivererByReferenceOrNameQuery, DelivererDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetDelivererByReferenceOrNameQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DelivererDto?> Handle(GetDelivererByReferenceOrNameQuery request, CancellationToken cancellationToken)
    {
        return await _context.Deliverers
             .AsNoTracking()
             .ProjectTo<DelivererDto>(_mapper.ConfigurationProvider)
             .FirstOrDefaultAsync(t => t.Reference!.ToLower() == request.term.ToLower() ||
                                          t.LastName!.ToLower() == request.term.ToLower(), cancellationToken);
    }
}
