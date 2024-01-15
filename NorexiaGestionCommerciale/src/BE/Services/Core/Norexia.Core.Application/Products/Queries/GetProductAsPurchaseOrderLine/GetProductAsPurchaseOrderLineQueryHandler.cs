using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.PurchaseOrders.Queries.GetPurchaseOrderLines;

namespace Norexia.Core.Application.Products.Queries.GetProductAsPurchaseOrderLine;
public record GetProductAsPurchaseOrderLineQuery(string Term) : IRequest<PurchaseOrderLineDto?>;
public class GetProductAsPurchaseOrderLineQueryHandler : IRequestHandler<GetProductAsPurchaseOrderLineQuery, PurchaseOrderLineDto?>
{
    private readonly IApplicationDbContext _context;

    public GetProductAsPurchaseOrderLineQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseOrderLineDto?> Handle(GetProductAsPurchaseOrderLineQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.SingleOrDefaultAsync(t => t.Reference!.ToLower() == request.Term.ToLower() || (t.Barcode != null && t.Barcode.ToLower() == request.Term.ToLower()));

        if (product == null)
            return null;

        var line = new PurchaseOrderLineDto();

        line.ProductId = product.Id;
        line.ShortDesignation = product!.ShortDesignation;
        line.Reference = product!.Reference;

        line.Qty = 1;
        line.Price = 0;
        line.VAT = 0;
        line.TotalPriceExcludingTax = 0;
        line.TotalVATPrice = 0;
        line.TotalPriceIncludingTax = 0;

        return line;
    }
}
