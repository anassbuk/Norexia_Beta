using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.ProviderInvoices.Queries.GetProviderInvoiceLines;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.ProviderInvoices.Queries.GetPurchaseOrderAsProviderInvoice;
public record GetPurchaseOrderAsProviderInvoiceQuery(string Term) : IRequest<PurchaseOrderAsProviderInvoiceDto?>;
public class GetPurchaseOrderAsProviderInvoiceQueryHandler : IRequestHandler<GetPurchaseOrderAsProviderInvoiceQuery, PurchaseOrderAsProviderInvoiceDto?>
{
    private readonly IApplicationDbContext _context;

    public GetPurchaseOrderAsProviderInvoiceQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseOrderAsProviderInvoiceDto?> Handle(GetPurchaseOrderAsProviderInvoiceQuery request, CancellationToken cancellationToken)
    {
        var purchase = await _context.PurchaseOrders
                                .Include(t => t.PurchaseOrderLines)!
                                .ThenInclude(l => l.Product)
                                .SingleOrDefaultAsync(t => t.Reference!.ToLower() == request.Term.ToLower());

        if (purchase == null)
            return null;

        var providerInvoice = new PurchaseOrderAsProviderInvoiceDto();

        var provider = _context.Providers.Find(purchase.ProviderId);
        string providerRef = $"{provider!.Reference} - {(provider!.ProviderType == ProviderType.Particular ? $"{provider!.FirstName}, {provider!.LastName}" : provider!.SocialReason)}";

        providerInvoice.Id = purchase.Id;
        providerInvoice.Reference = purchase.Reference;
        providerInvoice.ProviderId = purchase.ProviderId;
        providerInvoice.ProviderRef = providerRef;
        providerInvoice.ProviderInvoiceLines = new List<ProviderInvoiceLineDto>();

        foreach (var purchaseLine in purchase.PurchaseOrderLines!)
        {
            var line = new ProviderInvoiceLineDto
            {
                Id = Guid.NewGuid(),
                ProductId = purchaseLine.ProductId,
                Reference = purchaseLine.Product!.Reference,
                ShortDesignation = purchaseLine.Product!.ShortDesignation,
                ExpectedQty = purchaseLine.Qty,
                Qty = purchaseLine.Qty,
                Price = purchaseLine.Price,
                VAT = purchaseLine.VAT,
            };


            line.TotalPriceExcludingTax = line.Qty * line.Price;
            line.TotalVATPrice = line.TotalPriceExcludingTax * (((double?)line.VAT ?? 0) / 100);
            line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + line.TotalVATPrice;

            providerInvoice.ProviderInvoiceLines.Add(line);
        }

        return providerInvoice;
    }
}
