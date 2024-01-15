using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.PriceGroups.Queries.GetDefaultPriceGroup;
using Norexia.Core.Application.Quotations.Queries.GetQuotationsLines;

namespace Norexia.Core.Application.Products.Queries.GetProductAsQuotationLine;
public record GetProductAsQuotationLineQuery(string Term) : IRequest<QuotationLineDto?>;

internal class GetProductAsQuotationLineQueryHandler : IRequestHandler<GetProductAsQuotationLineQuery, QuotationLineDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public GetProductAsQuotationLineQueryHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<QuotationLineDto?> Handle(GetProductAsQuotationLineQuery request, CancellationToken cancellationToken)
    {
        var defaultPriceGroupId = await _mediator.Send(new GetDefaultPriceGroupQuery(), cancellationToken);
        var product = await _context.Products.SingleOrDefaultAsync(t => t.Reference!.ToLower() == request.Term.ToLower() || (t.Barcode != null && t.Barcode.ToLower() == request.Term.ToLower()));

        if (product == null)
            return null;

        var line = new QuotationLineDto();

        line.ProductId = product.Id;
        line.ShortDesignation = product!.ShortDesignation;
        line.Reference = product!.Reference;
        line.SellingPrices = _mapper.Map<ICollection<Commands.CreateProduct.SellingPriceDto>>(product.SellingPrices);

        var defaultSellingPrice = product.SellingPrices!.SingleOrDefault(t => t.PriceGroupId == defaultPriceGroupId);

        if (defaultSellingPrice == null)
            return null;

        line.Margin = defaultSellingPrice.Margin;
        line.Price = defaultSellingPrice.Price;
        line.Qty = 1;
        line.Discount = defaultSellingPrice.Discount ?? 0;
        line.SellingPriceId = defaultSellingPrice.Id;
        line.VAT = defaultSellingPrice.VAT ?? 0;

        line.TotalPriceExcludingTax = line.Qty * (line.Price - (line.Price * ((double)line.Discount / 100)));
        line.TotalVATPrice = line.TotalPriceExcludingTax * ((double)line.VAT! / 100);
        line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + line.TotalVATPrice;

        return line;
    }


}

