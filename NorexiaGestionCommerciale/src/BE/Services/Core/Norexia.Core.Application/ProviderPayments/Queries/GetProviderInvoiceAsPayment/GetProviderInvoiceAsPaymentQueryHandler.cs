using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.ProviderPayments.Queries.GetProviderInvoiceAsPayment;
public record GetProviderInvoiceAsPaymentQuery(string Term) : IRequest<ProviderInvoiceAsPaymentDto?>;
public class GetProviderInvoiceAsPaymentQueryHandler : IRequestHandler<GetProviderInvoiceAsPaymentQuery, ProviderInvoiceAsPaymentDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProviderInvoiceAsPaymentQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProviderInvoiceAsPaymentDto?> Handle(GetProviderInvoiceAsPaymentQuery request, CancellationToken cancellationToken)
    {
        var invoice = await _context.ProviderInvoices
                                .AsNoTracking()
                                .Where(c => c.IsDeleted == false && c.Reference!.ToLower() == request.Term.ToLower())
                                .Include(c => c.ProviderInvoiceLines)
                                .Include(c => c.ProviderInvoicePayments)
                                .FirstOrDefaultAsync(cancellationToken);

        if (invoice == null)
            return null;

        var invoiceAsPayment = _mapper.Map<ProviderInvoiceAsPaymentDto>(invoice);

        var totalPriceExcludingVAT = invoice.ProviderInvoiceLines!.Sum(line => line.Qty * (line.Price - (line.Price * (((double?)line.Discount ?? 0) / 100))));

        var taxPrice = invoice.ProviderInvoiceLines!.Sum(l => l.Qty * l.Price * (((double?)l.VAT ?? 0) / 100));

        invoiceAsPayment.TotalPriceIncludingVAT = totalPriceExcludingVAT + taxPrice;

        invoiceAsPayment.SettledAmount = invoice.ProviderInvoicePayments?.Where(p => !p.IsDeleted).Sum(p => p.AmountPaid ?? 0) ?? 0;
        invoiceAsPayment.RemainingAmount = invoiceAsPayment.TotalPriceIncludingVAT - invoiceAsPayment.SettledAmount;

        return invoiceAsPayment;
    }
}
