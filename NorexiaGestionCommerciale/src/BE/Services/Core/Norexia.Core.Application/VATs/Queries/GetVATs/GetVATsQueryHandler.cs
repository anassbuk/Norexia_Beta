using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.VATs.Queries.GetVATs;

public record GetVATsQuery : IRequest<IEnumerable<VATDto>>;
public class GetVATsQueryHandler : IRequestHandler<GetVATsQuery, IEnumerable<VATDto>>
{
    private IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetVATsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VATDto>> Handle(GetVATsQuery request, CancellationToken cancellationToken)
    {
        return await _context.VATs
                        .AsNoTracking()
                        .Where(c => c.IsDeleted == false)
                        .ProjectTo<VATDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}
