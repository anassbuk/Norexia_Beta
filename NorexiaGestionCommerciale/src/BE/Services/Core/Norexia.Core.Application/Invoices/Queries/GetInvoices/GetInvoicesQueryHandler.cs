using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.Invoices.Queries.GetInvoices;

public record GetInvoicesQuery : IRequest<IEnumerable<InvoiceDto>>;
public class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, IEnumerable<InvoiceDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetInvoicesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InvoiceDto>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        var invoices = await _context.Invoices
                                .AsNoTracking()
                                .Where(c => c.IsDeleted == false)
                                .Include(c => c.Customer)
                                .Include(c => c.PaymentTerms)
                                .Include(c => c.SaleOrder)
                                .Include(c => c.InvoiceLines)
                                .Include(c => c.InvoicePayments)
                                .ToListAsync(cancellationToken);

        var invoicesDto = new List<InvoiceDto>();

        foreach (var invoice in invoices)
        {
            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);

            if (invoice.InvoiceOrigin == InvoiceOrigin.SalesOrder || invoice.InvoiceOrigin == InvoiceOrigin.DeliveryMulti)
                invoiceDto.SaleOrderRef = invoice.SaleOrder!.Reference;
            else if (invoice.InvoiceOrigin == InvoiceOrigin.DeliveryMono)
                invoiceDto.DeliveryRef = invoice.DeliveryRef;

            invoiceDto.CustomerRef = $"{invoice.Customer!.Reference} - {(invoice.Customer!.ClientType == ClientType.Particular ? $"{invoice.Customer!.FirstName}, {invoice.Customer!.LastName}" : invoice.Customer!.SocialReason)}";

            var totalPriceExcludingVAT = invoice.InvoiceLines!.Select(line => line.Qty * (line.Price - (line.Price * (((double?)line.Discount ?? 0) / 100)))).Sum();

            var taxPrice = invoice.InvoiceLines!.Select(l => l.Qty * l.Price * (((double?)l.VAT ?? 0) / 100)).Sum();

            invoiceDto.TotalPriceIncludingVAT = totalPriceExcludingVAT + taxPrice;

            invoiceDto.SettledAmount = invoice.InvoicePayments?.Where(p => !p.IsDeleted).Select(p => p.AmountPaid ?? 0).Sum() ?? 0;
            invoiceDto.RemainingAmount = invoiceDto.TotalPriceIncludingVAT - invoiceDto.SettledAmount;

            invoicesDto.Add(invoiceDto);
        }

        return invoicesDto;
    }
}
