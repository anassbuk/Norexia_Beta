using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.Invoices.Queries.GetInvoiceAsPayment;
public record GetInvoiceAsPaymentQuery(string Term) : IRequest<InvoiceAsPaymentDto?>;
public class GetInvoiceAsPaymentQueryHandler : IRequestHandler<GetInvoiceAsPaymentQuery, InvoiceAsPaymentDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetInvoiceAsPaymentQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InvoiceAsPaymentDto?> Handle(GetInvoiceAsPaymentQuery request, CancellationToken cancellationToken)
    {
        var invoice = await _context.Invoices
                                .AsNoTracking()
                                .Where(c => c.IsDeleted == false && c.Reference!.ToLower() == request.Term.ToLower())
                                .Include(c => c.InvoiceLines)
                                .Include(c => c.InvoicePayments)
                                .FirstOrDefaultAsync(cancellationToken);

        if (invoice == null)
            return null;

        var invoiceAsPayment = _mapper.Map<InvoiceAsPaymentDto>(invoice);

        var totalPriceExcludingVAT = invoice.InvoiceLines!.Select(line => line.Qty * (line.Price - (line.Price * (((double?)line.Discount ?? 0) / 100)))).Sum();

        var taxPrice = invoice.InvoiceLines!.Select(l => l.Qty * l.Price * (((double?)l.VAT ?? 0) / 100)).Sum();

        invoiceAsPayment.TotalPriceIncludingVAT = totalPriceExcludingVAT + taxPrice;

        invoiceAsPayment.SettledAmount = invoice.InvoicePayments?.Where(p => !p.IsDeleted).Select(p => p.AmountPaid ?? 0).Sum() ?? 0;
        invoiceAsPayment.RemainingAmount = invoiceAsPayment.TotalPriceIncludingVAT - invoiceAsPayment.SettledAmount;

        return invoiceAsPayment;
    }
}
