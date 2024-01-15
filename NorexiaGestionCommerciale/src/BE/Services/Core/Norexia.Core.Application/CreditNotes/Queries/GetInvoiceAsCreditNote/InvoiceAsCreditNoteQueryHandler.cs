using AutoMapper;
using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Invoices.Queries.CreditNoteLineDto;
using Norexia.Core.Application.Products.Commands.CreateProduct;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.CreditNotes.Queries.GetInvoiceAsCreditNote;
public record GetInvoiceAsCreditNoteQuery(string Trem) : IRequest<InvoiceAsCreditNoteDto>;
public class GetInvoiceAsCreditNoteQueryHandler : IRequestHandler<GetInvoiceAsCreditNoteQuery, InvoiceAsCreditNoteDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInvoiceAsCreditNoteQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InvoiceAsCreditNoteDto> Handle(GetInvoiceAsCreditNoteQuery request, CancellationToken cancellationToken)
    {
       var invoice = await _context.Invoices
                                .Include(t => t.InvoiceLines)!
                                .ThenInclude(l => l.Product)
                                .ThenInclude(p => p!.SellingPrices)
                                .SingleOrDefaultAsync(t => t.Reference!.ToLower() == request.Trem.ToLower());



        if (invoice == null)
            throw new NotFoundException($"Invoice with reference ({request.Trem}) not found! ");

        var CreditNote = new InvoiceAsCreditNoteDto();
        var customer = _context.Customers.Find(invoice.CustomerId);
        string customerRef = string.Empty;
        if (customer != null)
            customerRef = $"{customer!.Reference} - {(customer!.ClientType == ClientType.Particular ? $"{customer!.FirstName}, {customer!.LastName}" : customer!.SocialReason)}";

        CreditNote.Id = invoice.Id;
        CreditNote.Reference = invoice.Reference;
        CreditNote.CustomerId = invoice.CustomerId;
        CreditNote.CustomerRef = customerRef;

        CreditNote.CreditNoteLines = new List<CreditNoteLineDto>();

       

        foreach (var saleLine in invoice.InvoiceLines!)
        {
            var line = new CreditNoteLineDto();

            line.Id = Guid.NewGuid();
            line.ProductId = saleLine.ProductId;
            line.SellingPriceId = saleLine.SellingPriceId;
            line.Reference = saleLine.Product!.Reference;
            line.ShortDesignation = saleLine.Product!.ShortDesignation;
            line.Qty = saleLine.Qty;
            line.ExpectedQty = saleLine.Qty;
            line.Price = saleLine.Price;
            line.Discount = saleLine.Discount ?? 0;
            line.VAT = saleLine.VAT ?? 0;


            line.TotalPriceExcludingTax = line.Qty * (line.Price - (line.Price * ((double)line.Discount / 100)));
            var totalVATPrice = line.TotalPriceExcludingTax * ((double)line.VAT! / 100);
            line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + totalVATPrice;
            line.SellingPrices = _mapper.Map<List<SellingPriceDto>>(saleLine.Product.SellingPrices!.ToList());

            CreditNote.CreditNoteLines.Add(line);


        }

        return CreditNote ;
    }
}
