using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Products.Commands.CreateProduct;
using Norexia.Core.Application.SaleOrders.Queries.GetSaleOrders;

namespace Norexia.Core.Application.SaleOrders.Queries.GetSaleOrderLines;
public class GetSaleOrderLinesQueryHandler : IRequestHandler<GetSaleOrderLinesQuery, IEnumerable<SaleOrderLineDto>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSaleOrderLinesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SaleOrderLineDto>?> Handle(GetSaleOrderLinesQuery request, CancellationToken cancellationToken)
    {
        List<SaleOrderLineDto> saleOrderLineDtos = new();
        var lines = await _context.SaleOrderLines
            .AsNoTracking()
            .Where(t => t.SaleOrderId == request.Id)
            .Include(l => l.Product)
            .ThenInclude(p => p!.SellingPrices)
            .ToListAsync(cancellationToken);

        foreach (var line in lines)
        {
            SaleOrderLineDto lineDto = _mapper.Map<SaleOrderLineDto>(line);
            lineDto.TotalPriceExcludingTax = line.Qty * (line.Price - (line.Price * (((double?)line.Discount ?? 0) / 100)));
            lineDto.TotalVATPrice = lineDto.TotalPriceExcludingTax * (((double?)line.VAT ?? 0) / 100);
            lineDto.TotalPriceIncludingTax = lineDto.TotalPriceExcludingTax + lineDto.TotalVATPrice;
            lineDto.ShortDesignation = line.Product!.ShortDesignation;
            lineDto.Reference = line.Product!.Reference;
            lineDto.SellingPrices = _mapper.Map<ICollection<SellingPriceDto>>(line.Product!.SellingPrices);

            saleOrderLineDtos.Add(lineDto);
        }

        return saleOrderLineDtos;
    }
}

public record GetSaleOrderLinesQuery(Guid Id) : IRequest<IEnumerable<SaleOrderLineDto>?>;
