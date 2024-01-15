using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Deliveries.Queries.GetDeliveryLines;
using Norexia.Core.Application.Products.Commands.CreateProduct;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.Invoices.Queries.GetInvoiceAsDelivery;
public record GetInvoiceAsDeliveryQuery(string Term) : IRequest<InvoiceAsDeliveryDto?>;
public class GetInvoiceAsDeliveryQueryHandler : IRequestHandler<GetInvoiceAsDeliveryQuery, InvoiceAsDeliveryDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInvoiceAsDeliveryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InvoiceAsDeliveryDto?> Handle(GetInvoiceAsDeliveryQuery request, CancellationToken cancellationToken)
    {
        var invoice = await _context.Invoices
                                .Include(t => t.InvoiceLines)!
                                .ThenInclude(l => l.Product)
                                .ThenInclude(p => p!.SellingPrices)
                                .SingleOrDefaultAsync(t => t.Reference!.ToLower() == request.Term.ToLower());

        if (invoice == null)
            return null;

        var delivery = new InvoiceAsDeliveryDto();

        var customer = _context.Customers.Find(invoice.CustomerId);
        string customerRef = string.Empty;
        if (customer != null)
            customerRef = $"{customer!.Reference} - {(customer!.ClientType == ClientType.Particular ? $"{customer!.FirstName}, {customer!.LastName}" : customer!.SocialReason)}";

        delivery.Id = invoice.Id;
        delivery.Reference = invoice.Reference;
        delivery.CustomerId = invoice.CustomerId;
        delivery.CustomerRef = customerRef;

        delivery.DeliveryLines = new List<DeliveryLineDto>();

        foreach (var saleLine in invoice.InvoiceLines!)
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
