using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Invoices.Queries.CreditNoteLineDto;
using Norexia.Core.Application.Products.Commands.CreateProduct;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.CreditNotes.Queries.GetCreditNoteLines;
internal class GetCreditNoteLinesQueryHandler : IRequestHandler<GetCreditNoteLinesQuery, IEnumerable<CreditNoteLineDto>>
{
    public readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCreditNoteLinesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<CreditNoteLineDto>> Handle(GetCreditNoteLinesQuery request, CancellationToken cancellationToken)
    {
        List<CreditNoteLineDto> creditNoteLineDtos = new();
       var lines = await _context.CreditNoteLines
            .AsNoTracking()
            .Where(t => t.CreditNoteId == request.Id)
            .Include(l => l.Product)
            .ThenInclude(p => p!.SellingPrices)
            .ToListAsync(cancellationToken);

        foreach (var line in lines)
        {
            CreditNoteLineDto lineDto = _mapper.Map<CreditNoteLineDto>(line);
            lineDto.ExpectedQty = line.ExpectedQty;
            lineDto.TotalPriceExcludingTax = line.Qty * (line.Price - (line.Price * (((double?)line.Discount ?? 0) / 100)));
            lineDto.TotalVATPrice = lineDto.TotalPriceExcludingTax * (((double?)line.VAT ?? 0) / 100);
            lineDto.TotalPriceIncludingTax = lineDto.TotalPriceExcludingTax + lineDto.TotalVATPrice;
            lineDto.ShortDesignation = line.Product!.ShortDesignation;
            lineDto.Reference = line.Product!.Reference;
            lineDto.SellingPrices = _mapper.Map<ICollection<SellingPriceDto>>(line.Product!.SellingPrices);

            lineDto.DeliveryRef = line.DeliveryRef;

            creditNoteLineDtos.Add(lineDto);
        }

        return creditNoteLineDtos;  

    }
}
public record GetCreditNoteLinesQuery(Guid Id) : IRequest<IEnumerable<CreditNoteLineDto>>;