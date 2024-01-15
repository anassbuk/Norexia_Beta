using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Products.Commands.CreateProduct;

namespace Norexia.Core.Application.Quotations.Queries.GetQuotationsLines;
public class GetQuotationLinesQueryHandler : IRequestHandler<GetQuotationsLinesQuery, IEnumerable<QuotationLineDto>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetQuotationLinesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<IEnumerable<QuotationLineDto>?> Handle(GetQuotationsLinesQuery request, CancellationToken cancellationToken)
    {
        List<QuotationLineDto> quotationLineDtos = new();
        var lines = await _context.QuotationsLines
        .AsNoTracking()
        .Where(t => t.QuotationID == request.QuotationId)
        .Include(l => l.Product)
        .ThenInclude(p => p!.SellingPrices)
        .ToListAsync(cancellationToken);

        foreach (var line in lines)
        {
            QuotationLineDto lineDto = _mapper.Map<QuotationLineDto>(line);
            lineDto.TotalPriceExcludingTax = line.Qty * (line.Price - (line.Price * (((double?)line.Discount ?? 0) / 100)));
            lineDto.TotalVATPrice = lineDto.TotalPriceExcludingTax * (((double?)line.VAT ?? 0) / 100);
            lineDto.TotalPriceIncludingTax = lineDto.TotalPriceExcludingTax + lineDto.TotalVATPrice;
            lineDto.ShortDesignation = line.Product!.ShortDesignation;
            lineDto.Reference = line.Product!.Reference;
            lineDto.SellingPrices = _mapper.Map<ICollection<SellingPriceDto>>(line.Product!.SellingPrices);

            quotationLineDtos.Add(lineDto); //Add the QuotationLineDto to the list
        }


        return quotationLineDtos; //Return the list
    }
}
public record GetQuotationsLinesQuery(Guid QuotationId) : IRequest<IEnumerable<QuotationLineDto>?>;

