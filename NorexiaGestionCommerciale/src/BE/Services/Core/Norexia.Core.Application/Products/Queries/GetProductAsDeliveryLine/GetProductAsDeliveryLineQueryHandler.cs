using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Deliveries.Queries.GetDeliveryLines;
using Norexia.Core.Application.PriceGroups.Queries.GetDefaultPriceGroup;
using Norexia.Core.Application.Products.Commands.CreateProduct;

namespace Norexia.Core.Application.Products.Queries.GetProductAsDeliveryLine;
public record GetProductAsDeliveryLineQuery(string Term) : IRequest<DeliveryLineDto?>;
public class GetProductAsDeliveryLineQueryHandler : IRequestHandler<GetProductAsDeliveryLineQuery, DeliveryLineDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public GetProductAsDeliveryLineQueryHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<DeliveryLineDto?> Handle(GetProductAsDeliveryLineQuery request, CancellationToken cancellationToken)
    {
        var defaultPriceGroupId = await _mediator.Send(new GetDefaultPriceGroupQuery(), cancellationToken);
        var product = await _context.Products.SingleOrDefaultAsync(t => t.Reference!.ToLower() == request.Term.ToLower() || (t.Barcode != null && t.Barcode.ToLower() == request.Term.ToLower()));

        if (product == null)
            return null;

        var line = new DeliveryLineDto();

        line.ProductId = product.Id;
        line.ShortDesignation = product!.ShortDesignation;
        line.Reference = product!.Reference;
        line.SellingPrices = _mapper.Map<ICollection<SellingPriceDto>>(product.SellingPrices);

        var defaultSellingPrice = product.SellingPrices!.SingleOrDefault(t => t.PriceGroupId == defaultPriceGroupId);

        if (defaultSellingPrice == null)
            return null;

        line.UnitPrice = defaultSellingPrice.Price;
        line.ExpectedQty = 0;
        line.Qty = 1;
        line.Discount = defaultSellingPrice.Discount ?? 0;
        line.SellingPriceId = defaultSellingPrice.Id;
        line.VAT = defaultSellingPrice.VAT ?? 0;

        line.TotalPriceExcludingTax = line.Qty * (line.UnitPrice - (line.UnitPrice * ((double)line.Discount / 100)));
        var totalVATPrice = line.TotalPriceExcludingTax * ((double)line.VAT! / 100);
        line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + totalVATPrice;

        return line;
    }
}
