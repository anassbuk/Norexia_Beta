using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.PurchaseOrders.Queries.GetPurchaseOrderLines;
public class GetPurchaseOrderLinesQueryHandler : IRequestHandler<GetPurchaseOrderLinesQuery, IEnumerable<PurchaseOrderLineDto>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPurchaseOrderLinesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PurchaseOrderLineDto>?> Handle(GetPurchaseOrderLinesQuery request, CancellationToken cancellationToken)
    {
        List<PurchaseOrderLineDto> purchaseOrderLineDtos = new();
        var lines = await _context.PurchaseOrderLines
                .AsNoTracking()
                .Where(t => t.PurchaseOrderId == request.Id)
                .Include(l => l.Product)
                .ToListAsync(cancellationToken);

        foreach (var line in lines)
        {
            PurchaseOrderLineDto lineDto = _mapper.Map<PurchaseOrderLineDto>(line);

            lineDto.TotalPriceExcludingTax = line.Qty * line.Price;
            lineDto.TotalVATPrice = lineDto.TotalPriceExcludingTax * (((double?)line.VAT ?? 0) / 100);
            lineDto.TotalPriceIncludingTax = lineDto.TotalPriceExcludingTax + lineDto.TotalVATPrice;
            lineDto.ShortDesignation = line.Product!.ShortDesignation;
            lineDto.Reference = line.Product!.Reference;

            purchaseOrderLineDtos.Add(lineDto);
        }

        return purchaseOrderLineDtos;
    }
}

public record GetPurchaseOrderLinesQuery(Guid Id) : IRequest<IEnumerable<PurchaseOrderLineDto>?>;
