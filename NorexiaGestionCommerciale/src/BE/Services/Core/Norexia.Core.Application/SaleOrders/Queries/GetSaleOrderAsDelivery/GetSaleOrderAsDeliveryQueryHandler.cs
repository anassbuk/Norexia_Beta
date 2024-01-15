using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Deliveries.Queries.GetDeliveryLines;
using Norexia.Core.Application.Products.Commands.CreateProduct;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.SaleOrders.Queries.GetSaleOrderAsDelivery;
public record GetSaleOrderAsDeliveryQuery(string Term) : IRequest<SaleOrderAsDeliveryDto?>;
public class GetSaleOrderAsDeliveryQueryHandler : IRequestHandler<GetSaleOrderAsDeliveryQuery, SaleOrderAsDeliveryDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSaleOrderAsDeliveryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context; _mapper = mapper;
    }

    public async Task<SaleOrderAsDeliveryDto?> Handle(GetSaleOrderAsDeliveryQuery request, CancellationToken cancellationToken)
    {
        var sale = await _context.SaleOrders
                                .Include(t => t.SaleOrderLines)!
                                .ThenInclude(l => l.Product)
                                .ThenInclude(p => p!.SellingPrices)
                                .SingleOrDefaultAsync(t => t.Reference!.ToLower() == request.Term.ToLower());

        if (sale == null)
            return null;

        var delivery = new SaleOrderAsDeliveryDto();

        var customer = _context.Customers.Find(sale.CustomerId);
        string customerRef = $"{customer!.Reference} - {(customer!.ClientType == ClientType.Particular ? $"{customer!.FirstName}, {customer!.LastName}" : customer!.SocialReason)}";

        delivery.Id = sale.Id;
        delivery.Reference = sale.Reference;
        delivery.CustomerId = sale.CustomerId;
        delivery.PlannedDate = sale.DeliveryDate;
        delivery.DeliveryMode = sale.DeliveryMode;
        delivery.CustomerRef = customerRef;

        delivery.DeliveryLines = new List<DeliveryLineDto>();

        foreach (var saleLine in sale.SaleOrderLines!)
        {
            var line = new DeliveryLineDto();

            line.Id = Guid.NewGuid();
            line.ProductId = saleLine.ProductId;
            line.SellingPriceId = saleLine.SellingPriceId;
            line.Reference = saleLine.Product!.Reference;
            line.ShortDesignation = saleLine.Product!.ShortDesignation;
            line.Qty = saleLine.Qty;
            line.ExpectedQty = saleLine.Qty;
            line.UnitPrice = saleLine.Price;
            line.Discount = saleLine.Discount ?? 0;
            line.VAT = saleLine.VAT ?? 0;


            line.TotalPriceExcludingTax = line.Qty * (line.UnitPrice - (line.UnitPrice * ((double)line.Discount / 100)));
            var totalVATPrice = line.TotalPriceExcludingTax * ((double)line.VAT! / 100);
            line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + totalVATPrice;

            line.SellingPrices = _mapper.Map<List<SellingPriceDto>>(saleLine.Product.SellingPrices!.ToList());

            delivery.DeliveryLines.Add(line);
        }

        return delivery;
    }
}
