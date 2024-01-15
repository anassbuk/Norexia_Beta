using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.Payments.Queries.GetPayments;
public record GetPaymentsQuery : IRequest<IEnumerable<PaymentDto>?>;
public class GetPaymentsQueryHandler : IRequestHandler<GetPaymentsQuery, IEnumerable<PaymentDto>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPaymentsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PaymentDto>?> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
    {
        List<PaymentDto> paymentDtos = new();

        var invoicePayments = await _context.InvoicePayments
            .AsNoTracking()
            .Where(t => !t.IsDeleted)
            .Include(l => l.Invoice)
            .Include(l => l.PaymentMean)
            .ToListAsync(cancellationToken);

        var salePayments = await _context.SalePayments
            .AsNoTracking()
            .Where(t => !t.IsDeleted)
            .Include(l => l.SaleOrder)
            .Include(l => l.PaymentMean)
            .ToListAsync(cancellationToken);

        foreach (var payment in invoicePayments)
        {
            PaymentDto paymentDto = _mapper.Map<PaymentDto>(payment);

            paymentDto.InvoiceRef = payment.Invoice?.Reference;
            paymentDto.PaymentMeanName = payment.PaymentMean?.Name;
            paymentDto.PaymentOrigin = Domain.Common.Enums.PaymentOrigin.Invoice;

            paymentDtos.Add(paymentDto);
        }

        foreach (var payment in salePayments)
        {
            PaymentDto paymentDto = _mapper.Map<PaymentDto>(payment);

            paymentDto.SaleOrderRef = payment.SaleOrder?.Reference;
            paymentDto.PaymentMeanName = payment.PaymentMean?.Name;
            paymentDto.PaymentOrigin = Domain.Common.Enums.PaymentOrigin.SaleOrder;

            paymentDtos.Add(paymentDto);
        }

        return paymentDtos;
    }
}
