using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.Deliverers.Queries.GetDeliverers;
public record GetDeliverersQuery : IRequest<IEnumerable<DelivererDto>>;
public class GetDeliverersQueryHandler : IRequestHandler<GetDeliverersQuery, IEnumerable<DelivererDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetDeliverersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DelivererDto>> Handle(GetDeliverersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Deliverers
                        .AsNoTracking()
                        .Where(c => !c.IsDeleted)
                        .ProjectTo<DelivererDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}
