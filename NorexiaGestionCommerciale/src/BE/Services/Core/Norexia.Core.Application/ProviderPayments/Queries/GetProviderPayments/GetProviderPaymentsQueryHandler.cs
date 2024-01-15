using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.ProviderPayments.Queries.GetProviderPayments;
public record GetProviderPaymentsQuery : IRequest<IEnumerable<ProviderInvoicePaymentDto>?>;
public class GetProviderPaymentsQueryHandler : IRequestHandler<GetProviderPaymentsQuery, IEnumerable<ProviderInvoicePaymentDto>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProviderPaymentsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProviderInvoicePaymentDto>?> Handle(GetProviderPaymentsQuery request, CancellationToken cancellationToken)
    {
        List<ProviderInvoicePaymentDto> paymentDtos = new();

        var invoicePayments = await _context.ProviderInvoicePayments
            .AsNoTracking()
            .Where(t => !t.IsDeleted)
            .Include(l => l.ProviderInvoice)
            .Include(l => l.PaymentMean)
        .ToListAsync(cancellationToken);

        foreach (var payment in invoicePayments)
        {
            var paymentDto = _mapper.Map<ProviderInvoicePaymentDto>(payment);

            paymentDto.ProviderInvoiceRef = payment.ProviderInvoice?.Reference;
            paymentDto.PaymentMeanName = payment.PaymentMean?.Name;

            paymentDtos.Add(paymentDto);
        }

        return paymentDtos;
    }
}
