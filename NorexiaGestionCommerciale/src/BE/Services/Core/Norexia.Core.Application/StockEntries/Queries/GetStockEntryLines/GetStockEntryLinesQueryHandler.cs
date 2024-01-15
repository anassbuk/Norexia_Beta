using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.StockEntries.Queries.GetStockEntryLines;
public record GetStockEntryLinesQuery(Guid Id) : IRequest<IEnumerable<StockEntryLineDto>>;
public class GetStockEntryLinesQueryHandler : IRequestHandler<GetStockEntryLinesQuery, IEnumerable<StockEntryLineDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetStockEntryLinesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StockEntryLineDto>> Handle(GetStockEntryLinesQuery request, CancellationToken cancellationToken)
    {
        var stockEntry = await _context.StockEntries
                                .Include(e => e.StockEntryLines!)
                                .ThenInclude(l => l.Product)
                                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (stockEntry == null)
            throw new NotFoundException($"Stock entry with id ({request.Id}) not found!");

        List<StockEntryLineDto> lines = new();

        foreach (var line in stockEntry.StockEntryLines!)
        {
            var lineDto = _mapper.Map<StockEntryLineDto>(line);
            lineDto.ShortDesignation = line.Product!.ShortDesignation;
            lineDto.Reference = line.Product!.Reference;
            lineDto.ExpectedQty = _context.PurchaseOrders
                                    .Find(stockEntry.PurchaseOrderId)!.PurchaseOrderLines!
                                    .Single(l => l.ProductId == line.ProductId).Qty;

            lines.Add(lineDto);
        }

        return lines;
    }
}
