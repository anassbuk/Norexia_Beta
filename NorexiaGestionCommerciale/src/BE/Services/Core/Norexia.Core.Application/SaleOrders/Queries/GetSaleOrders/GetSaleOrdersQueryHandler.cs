using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.SaleOrders.Queries.GetSaleOrders;
public record GetSaleOrdersQuery : IRequest<IEnumerable<SaleOrderDto>>;
public class GetSaleOrdersQueryHandler : IRequestHandler<GetSaleOrdersQuery, IEnumerable<SaleOrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSaleOrdersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SaleOrderDto>> Handle(GetSaleOrdersQuery request, CancellationToken cancellationToken)
    {
        List<SaleOrderDto> SaleOrdersDto = new();

        var saleOrders = await _context.SaleOrders
                                .AsNoTracking()
                                .Where(c => c.IsDeleted == false)
                                .Include(t => t.SaleOrderLines)
                                .Include(t => t.PaymentTerms)
                                .Include(t => t.Quotation)
                                .ToListAsync(cancellationToken);

        foreach (var saleOrder in saleOrders)
        {
            var saleOrderDto = _mapper.Map<SaleOrderDto>(saleOrder);

            saleOrderDto.QuotationRef = saleOrder.Quotation?.Reference;

            saleOrderDto.PriceExcludingTax = saleOrder.SaleOrderLines!.Select(line => line.Qty * (line.Price - (line.Price * (((double?)line.Discount ?? 0) / 100)))).Sum();

            saleOrderDto.TaxPrice = saleOrder.SaleOrderLines!.Select(l => (l.Qty * l.Price) * (((double?)l.VAT ?? 0) / 100)).Sum();

            saleOrderDto.PriceIncludingTax = saleOrderDto.PriceExcludingTax + saleOrderDto.TaxPrice;

            saleOrderDto.DiscountPrice = saleOrderDto.PriceExcludingTax * (((double?)saleOrderDto.Discount ?? 0) / 100);

            saleOrderDto.NetPrice = saleOrderDto.PriceIncludingTax - saleOrderDto.DiscountPrice;

            SaleOrdersDto.Add(saleOrderDto);
        }

        return SaleOrdersDto;
    }
}
