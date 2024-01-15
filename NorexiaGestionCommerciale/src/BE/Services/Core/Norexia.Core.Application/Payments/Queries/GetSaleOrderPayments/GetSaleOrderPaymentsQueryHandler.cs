using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Payments.Queries.GetPayments;

namespace Norexia.Core.Application.Payments.Queries.GetSaleOrderPayments;
public record GetSaleOrderPaymentsQuery(Guid Id) : IRequest<IEnumerable<PaymentDto>?>;
public class GetSaleOrderPaymentsQueryHandler : IRequestHandler<GetSaleOrderPaymentsQuery, IEnumerable<PaymentDto>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSaleOrderPaymentsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PaymentDto>?> Handle(GetSaleOrderPaymentsQuery request, CancellationToken cancellationToken)
    {
        List<PaymentDto> paymentDtos = new();

        var payments = await _context.SalePayments
            .AsNoTracking()
            .Where(t => t.SaleOrderId == request.Id && !t.IsDeleted)
            .Include(l => l.SaleOrder)
            .Include(l => l.PaymentMean)
            .ToListAsync(cancellationToken);

        foreach (var payment in payments)
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
