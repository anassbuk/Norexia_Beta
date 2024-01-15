using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.StockEntries.Queries.GetStockEntryLines;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.PurchaseOrders.Queries.GetPurchaseOrderAsStockEntry;
public record GetPurchaseOrderAsStockEntryQuery(string Term) : IRequest<PurchaseOrderAsStockEntryDto?>;
public class GetPurchaseOrderAsStockEntryQueryHandler : IRequestHandler<GetPurchaseOrderAsStockEntryQuery, PurchaseOrderAsStockEntryDto?>
{
    private readonly IApplicationDbContext _context;

    public GetPurchaseOrderAsStockEntryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseOrderAsStockEntryDto?> Handle(GetPurchaseOrderAsStockEntryQuery request, CancellationToken cancellationToken)
    {
        var purchase = await _context.PurchaseOrders
                                .Include(t => t.PurchaseOrderLines)!
                                .ThenInclude(l => l.Product)
                                .SingleOrDefaultAsync(t => t.Reference!.ToLower() == request.Term.ToLower());

        if (purchase == null)
            return null;

        var stockEntry = new PurchaseOrderAsStockEntryDto();

        var provider = _context.Providers.Find(purchase.ProviderId);
        string providerRef = $"{provider!.Reference} - {(provider!.ProviderType == ProviderType.Particular ? $"{provider!.FirstName}, {provider!.LastName}" : provider!.SocialReason)}";

        stockEntry.Id = purchase.Id;
        stockEntry.Reference = purchase.Reference;
        stockEntry.ProviderId = purchase.ProviderId;
        stockEntry.ProviderRef = providerRef;
        stockEntry.StockEntryLines = new List<StockEntryLineDto>();

        foreach (var purchaseLine in purchase.PurchaseOrderLines!)
        {
            var line = new StockEntryLineDto();
            line.Id = Guid.NewGuid();
            line.ProductId = purchaseLine.ProductId;
            line.Reference = purchaseLine.Product!.Reference;
            line.ShortDesignation = purchaseLine.Product!.ShortDesignation;
            line.ExpectedQty = purchaseLine.Qty;
            line.Qty = purchaseLine.Qty;

            stockEntry.StockEntryLines.Add(line);
        }

        return stockEntry;
    }
}
