using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.StockEntries.Queries.GetStockEntries;
public record GetStockEntriesQuery : IRequest<IEnumerable<StockEntryDto>>;
public class GetStockEntriesQueryHandler : IRequestHandler<GetStockEntriesQuery, IEnumerable<StockEntryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetStockEntriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StockEntryDto>> Handle(GetStockEntriesQuery request, CancellationToken cancellationToken)
    {
        var stockEntries = await _context.StockEntries
                                .AsNoTracking()
                                .Where(c => c.IsDeleted == false)
                                .ProjectTo<StockEntryDto>(_mapper.ConfigurationProvider)
                                .ToListAsync(cancellationToken);

        foreach (var stockEntry in stockEntries)
        {
            stockEntry.PurchaseOrderRef = _context.PurchaseOrders.Find(stockEntry.PurchaseOrderId)!.Reference;
            var provider = _context.Providers.Find(stockEntry.ProviderId);
            string providerRef = $"{provider!.Reference} - {(provider!.ProviderType == ProviderType.Particular ? $"{provider!.FirstName}, {provider!.LastName}" : provider!.SocialReason)}";
            stockEntry.ProviderRef = providerRef;
        }

        return stockEntries;
    }
}
