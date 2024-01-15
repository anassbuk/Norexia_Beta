using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Products.Commands.CreateProduct;

namespace Norexia.Core.Application.Invoices.Queries.GetInvoiceLines;

public class GetInvoiceLinesQueryHandler : IRequestHandler<GetInvoiceLinesQuery, IEnumerable<InvoiceLineDto>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInvoiceLinesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InvoiceLineDto>?> Handle(GetInvoiceLinesQuery request, CancellationToken cancellationToken)
    {
        List<InvoiceLineDto> invoiceLineDtos = new();
        var lines = await _context.InvoiceLines
            .AsNoTracking()
            .Where(t => t.InvoiceId == request.Id)
            .Include(l => l.Product)
            .ThenInclude(p => p!.SellingPrices)
            .ToListAsync(cancellationToken);

        foreach (var line in lines)
        {
            InvoiceLineDto lineDto = _mapper.Map<InvoiceLineDto>(line);
            lineDto.ExpectedQty = line.ExpectedQty;
            lineDto.TotalPriceExcludingTax = line.Qty * (line.Price - (line.Price * (((double?)line.Discount ?? 0) / 100)));
            lineDto.TotalVATPrice = lineDto.TotalPriceExcludingTax * (((double?)line.VAT ?? 0) / 100);
            lineDto.TotalPriceIncludingTax = lineDto.TotalPriceExcludingTax + lineDto.TotalVATPrice;
            lineDto.ShortDesignation = line.Product!.ShortDesignation;
            lineDto.Reference = line.Product!.Reference;
            lineDto.SellingPrices = _mapper.Map<ICollection<SellingPriceDto>>(line.Product!.SellingPrices);

            lineDto.DeliveryRef = line.DeliveryRef;

            invoiceLineDtos.Add(lineDto);
        }

        return invoiceLineDtos;
    }
}
public record GetInvoiceLinesQuery(Guid Id) : IRequest<IEnumerable<InvoiceLineDto>?>;
