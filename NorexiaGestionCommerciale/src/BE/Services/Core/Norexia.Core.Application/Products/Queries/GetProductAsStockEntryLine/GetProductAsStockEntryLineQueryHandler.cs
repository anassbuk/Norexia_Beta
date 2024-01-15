using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.StockEntries.Queries.GetStockEntryLines;

namespace Norexia.Core.Application.Products.Queries.GetProductAsStockEntryLine;
public record GetProductAsStockEntryLineQuery(string Term) : IRequest<StockEntryLineDto?>;
public class GetProductAsStockEntryLineQueryHandler : IRequestHandler<GetProductAsStockEntryLineQuery, StockEntryLineDto?>
{
    private readonly IApplicationDbContext _context;

    public GetProductAsStockEntryLineQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StockEntryLineDto?> Handle(GetProductAsStockEntryLineQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.SingleOrDefaultAsync(t => t.Reference!.ToLower() == request.Term.ToLower() || (t.Barcode != null && t.Barcode.ToLower() == request.Term.ToLower()));

        if (product == null)
            return null;

        var line = new StockEntryLineDto()
        {
            ProductId = product.Id,
            ShortDesignation = product!.ShortDesignation,
            Reference = product!.Reference,

            ExpectedQty = 0,
            Qty = 1,
        };

        return line;
    }
}
