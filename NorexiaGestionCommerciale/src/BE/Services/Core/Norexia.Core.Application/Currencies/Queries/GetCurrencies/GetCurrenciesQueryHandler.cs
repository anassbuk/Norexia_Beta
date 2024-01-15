using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.Currencies.Queries.GetCurrencies;

public record GetCurrenciesQuery : IRequest<IEnumerable<CurrencyDto>>;
public class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, IEnumerable<CurrencyDto>>
{
    private IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetCurrenciesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CurrencyDto>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Currencies
                        .AsNoTracking()
                        .Where(c => c.IsDeleted == false)
                        .ProjectTo<CurrencyDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}
