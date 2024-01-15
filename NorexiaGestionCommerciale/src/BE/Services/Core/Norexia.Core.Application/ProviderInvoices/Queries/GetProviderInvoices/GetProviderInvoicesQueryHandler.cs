using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.ProviderInvoices.Queries.GetProviderInvoices;
public record GetProviderInvoicesQuery : IRequest<IEnumerable<ProviderInvoiceDto>>;
public class GetProviderInvoicesQueryHandler : IRequestHandler<GetProviderInvoicesQuery, IEnumerable<ProviderInvoiceDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetProviderInvoicesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProviderInvoiceDto>> Handle(GetProviderInvoicesQuery request, CancellationToken cancellationToken)
    {
        var invoices = await _context.ProviderInvoices
                                .AsNoTracking()
                                .Where(c => c.IsDeleted == false)
                                .Include(c => c.Provider)
                                .Include(c => c.PaymentTerms)
                                .Include(c => c.PurchaseOrder)
                                .Include(c => c.ProviderInvoiceLines)
                                .Include(c => c.ProviderInvoicePayments)
                                .ToListAsync(cancellationToken);

        var invoicesDto = new List<ProviderInvoiceDto>();

        foreach (var invoice in invoices)
        {
            var invoiceDto = _mapper.Map<ProviderInvoiceDto>(invoice);

            if (invoice.ProviderInvoiceOrigin == ProviderInvoiceOrigin.PurchaseOrder)
                invoiceDto.PurchaseOrderRef = invoice.PurchaseOrder!.Reference;

            invoiceDto.ProviderRef = $"{invoice.Provider!.Reference} - {(invoice.Provider!.ProviderType == ProviderType.Particular ? $"{invoice.Provider!.FirstName}, {invoice.Provider!.LastName}" : invoice.Provider!.SocialReason)}";

            var totalPriceExcludingVAT = invoice.ProviderInvoiceLines!.Sum(line => line.Qty * (line.Price - (line.Price * (((double?)line.Discount ?? 0) / 100))));

            var taxPrice = invoice.ProviderInvoiceLines!.Sum(l => l.Qty * l.Price * (((double?)l.VAT ?? 0) / 100));

            invoiceDto.TotalPriceIncludingVAT = totalPriceExcludingVAT + taxPrice;

            invoiceDto.SettledAmount = invoice.ProviderInvoicePayments?.Where(p => !p.IsDeleted).Sum(p => p.AmountPaid ?? 0) ?? 0;
            invoiceDto.RemainingAmount = invoiceDto.TotalPriceIncludingVAT - invoiceDto.SettledAmount;

            invoicesDto.Add(invoiceDto);
        }

        return invoicesDto;
    }
}
