using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.ProviderInvoices.Queries.GetProviderInvoiceLines;

namespace Norexia.Core.Application.ProviderInvoices.Queries.GetProductAsProviderInvoiceLine;
public record GetProductAsProviderInvoiceLineQuery(string Term) : IRequest<ProviderInvoiceLineDto?>;
public class GetProductAsProviderInvoiceLineQueryHandler : IRequestHandler<GetProductAsProviderInvoiceLineQuery, ProviderInvoiceLineDto?>
{
    private readonly IApplicationDbContext _context;

    public GetProductAsProviderInvoiceLineQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProviderInvoiceLineDto?> Handle(GetProductAsProviderInvoiceLineQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.SingleOrDefaultAsync(t => t.Reference!.ToLower() == request.Term.ToLower() || (t.Barcode != null && t.Barcode.ToLower() == request.Term.ToLower()));

        if (product == null)
            return null;

        var line = new ProviderInvoiceLineDto();

        line.ProductId = product.Id;
        line.ShortDesignation = product!.ShortDesignation;
        line.Reference = product!.Reference;

        line.Qty = 1;
        line.ExpectedQty = 0;
        line.Price = 0;
        line.VAT = 0;
        line.TotalPriceExcludingTax = 0;
        line.TotalVATPrice = 0;
        line.TotalPriceIncludingTax = 0;

        return line;
    }
}
