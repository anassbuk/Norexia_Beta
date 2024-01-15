using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Payments.Queries.GetPayments;

namespace Norexia.Core.Application.Payments.Queries.GetInvoicePayments;
public record GetInvoicePaymentsQuery(Guid Id) : IRequest<IEnumerable<PaymentDto>?>;
public class GetInvoicePaymentsQueryHandler : IRequestHandler<GetInvoicePaymentsQuery, IEnumerable<PaymentDto>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInvoicePaymentsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<PaymentDto>?> Handle(GetInvoicePaymentsQuery request, CancellationToken cancellationToken)
    {
        List<PaymentDto> paymentDtos = new();

        var payments = await _context.InvoicePayments
            .AsNoTracking()
            .Where(t => t.InvoiceId == request.Id && !t.IsDeleted)
            .Include(l => l.Invoice)
            .Include(l => l.PaymentMean)
            .ToListAsync(cancellationToken);

        foreach (var payment in payments)
        {
            PaymentDto paymentDto = _mapper.Map<PaymentDto>(payment);

            paymentDto.InvoiceRef = payment.Invoice?.Reference;
            paymentDto.PaymentMeanName = payment.PaymentMean?.Name;
            paymentDto.PaymentOrigin = Domain.Common.Enums.PaymentOrigin.Invoice;

            paymentDtos.Add(paymentDto);
        }

        return paymentDtos;
    }
}
