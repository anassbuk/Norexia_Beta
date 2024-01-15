using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.ProviderInvoices.Queries.GetProviderInvoiceLines;
public record GetProviderInvoiceLinesQuery(Guid Id) : IRequest<IEnumerable<ProviderInvoiceLineDto>?>;
public class GetProviderInvoiceLinesQueryHandler : IRequestHandler<GetProviderInvoiceLinesQuery, IEnumerable<ProviderInvoiceLineDto>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProviderInvoiceLinesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<ProviderInvoiceLineDto>?> Handle(GetProviderInvoiceLinesQuery request, CancellationToken cancellationToken)
    {
        List<ProviderInvoiceLineDto> invoiceLineDtos = new();
        var lines = await _context.ProviderInvoiceLines
                .AsNoTracking()
                .Where(t => t.ProviderInvoiceId == request.Id)
                .Include(l => l.Product)
                .ToListAsync(cancellationToken);

        foreach (var line in lines)
        {
            ProviderInvoiceLineDto lineDto = _mapper.Map<ProviderInvoiceLineDto>(line);

            lineDto.TotalPriceExcludingTax = line.Qty * line.Price;
            lineDto.TotalVATPrice = lineDto.TotalPriceExcludingTax * (((double?)line.VAT ?? 0) / 100);
            lineDto.TotalPriceIncludingTax = lineDto.TotalPriceExcludingTax + lineDto.TotalVATPrice;
            lineDto.ShortDesignation = line.Product!.ShortDesignation;
            lineDto.Reference = line.Product!.Reference;

            invoiceLineDtos.Add(lineDto);
        }

        return invoiceLineDtos;
    }
}
