using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.Payments.Queries.GetSaleOrderAsPayment;
public record GetSaleOrderAsPaymentQuery(string Term) : IRequest<SaleOrderAsPaymentDto?>;
public class GetSaleOrderAsPaymentQueryHandler : IRequestHandler<GetSaleOrderAsPaymentQuery, SaleOrderAsPaymentDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetSaleOrderAsPaymentQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SaleOrderAsPaymentDto?> Handle(GetSaleOrderAsPaymentQuery request, CancellationToken cancellationToken)
    {
        var sale = await _context.SaleOrders
                                .AsNoTracking()
                                .Where(c => c.IsDeleted == false && c.Reference!.ToLower() == request.Term.ToLower())
                                .Include(c => c.SaleOrderLines)
                                .Include(c => c.SalePayments)
                                .FirstOrDefaultAsync(cancellationToken);

        if (sale == null)
            return null;

        var saleOrderAsPayment = _mapper.Map<SaleOrderAsPaymentDto>(sale);

        var totalPriceExcludingVAT = sale.SaleOrderLines!.Select(line => line.Qty * (line.Price - (line.Price * (((double?)line.Discount ?? 0) / 100)))).Sum();

        var taxPrice = sale.SaleOrderLines!.Select(l => l.Qty * l.Price * (((double?)l.VAT ?? 0) / 100)).Sum();
        
        saleOrderAsPayment.EntryDate = sale.OrderDate;
        saleOrderAsPayment.TotalPriceIncludingVAT = totalPriceExcludingVAT + taxPrice;

        saleOrderAsPayment.SettledAmount = sale.SalePayments?.Where(p => !p.IsDeleted).Select(p => p.AmountPaid ?? 0).Sum() ?? 0;
        saleOrderAsPayment.RemainingAmount = saleOrderAsPayment.TotalPriceIncludingVAT - saleOrderAsPayment.SettledAmount;

        return saleOrderAsPayment;
    }
}
