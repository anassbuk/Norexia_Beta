using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.PurchaseOrders.Queries.GetPurchaseOrders;
public record GetPurchaseOrdersQuery : IRequest<IEnumerable<PurchaseOrderDto>>;
public class GetPurchaseOrdersQueryHandler : IRequestHandler<GetPurchaseOrdersQuery, IEnumerable<PurchaseOrderDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPurchaseOrdersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PurchaseOrderDto>> Handle(GetPurchaseOrdersQuery request, CancellationToken cancellationToken)
    {
        List<PurchaseOrderDto> purchaseOrdersDto = new();

        var purchaseOrders = await _context.PurchaseOrders
                                .AsNoTracking()
                                .Where(c => c.IsDeleted == false)
                                .Include(t => t.PurchaseOrderLines)
                                .ToListAsync(cancellationToken);

        foreach (var purchaseOrder in purchaseOrders)
        {
            PurchaseOrderDto purchaseOrderDto = new()
            {
                Id = purchaseOrder.Id,
                ProviderId = purchaseOrder.ProviderId,
                Reference = purchaseOrder.Reference,
                OrderDate = purchaseOrder.OrderDate,
                Note = purchaseOrder.Note,
            };

            purchaseOrderDto.PriceExcludingTax = purchaseOrder.PurchaseOrderLines!.Select(l => l.Qty * l.Price).Sum();

            purchaseOrderDto.TaxPrice = purchaseOrder.PurchaseOrderLines!.Select(l => l.Qty * l.Price * (((double?)l.VAT ?? 0) / 100)).Sum();

            purchaseOrderDto.PriceIncludingTax = purchaseOrderDto.PriceExcludingTax + purchaseOrderDto.TaxPrice;

            purchaseOrderDto.NetPrice = purchaseOrderDto.PriceIncludingTax;

            purchaseOrdersDto.Add(purchaseOrderDto);
        }

        return purchaseOrdersDto;
    }
}
