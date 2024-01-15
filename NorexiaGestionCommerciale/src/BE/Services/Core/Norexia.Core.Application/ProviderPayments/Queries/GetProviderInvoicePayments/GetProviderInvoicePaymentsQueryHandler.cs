using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.ProviderPayments.Queries.GetProviderPayments;

namespace Norexia.Core.Application.ProviderPayments.Queries.GetProviderInvoicePayments;
public record GetProviderInvoicePaymentsQuery(Guid Id) : IRequest<IEnumerable<ProviderInvoicePaymentDto>?>;
public class GetProviderInvoicePaymentsQueryHandler : IRequestHandler<GetProviderInvoicePaymentsQuery, IEnumerable<ProviderInvoicePaymentDto>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProviderInvoicePaymentsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProviderInvoicePaymentDto>?> Handle(GetProviderInvoicePaymentsQuery request, CancellationToken cancellationToken)
    {
        List<ProviderInvoicePaymentDto> paymentDtos = new();

        var payments = await _context.ProviderInvoicePayments
            .AsNoTracking()
            .Where(t => t.ProviderInvoiceId == request.Id && !t.IsDeleted)
            .Include(l => l.ProviderInvoice)
            .Include(l => l.PaymentMean)
            .ToListAsync(cancellationToken);

        foreach (var payment in payments)
        {
            var paymentDto = _mapper.Map<ProviderInvoicePaymentDto>(payment);

            paymentDto.ProviderInvoiceRef = payment.ProviderInvoice?.Reference;
            paymentDto.PaymentMeanName = payment.PaymentMean?.Name;

            paymentDtos.Add(paymentDto);
        }

        return paymentDtos;
    }
}
