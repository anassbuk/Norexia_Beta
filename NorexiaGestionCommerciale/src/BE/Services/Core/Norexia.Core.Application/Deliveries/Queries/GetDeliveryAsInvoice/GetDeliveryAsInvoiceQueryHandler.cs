using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Invoices.Queries.GetInvoiceLines;
using Norexia.Core.Application.Products.Commands.CreateProduct;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.Deliveries.Queries.GetDeliveryAsInvoice;
public record GetDeliveryAsInvoiceQuery(string Term) : IRequest<DeliveryAsInvoiceDto?>;
public class GetDeliveryAsInvoiceQueryHandler : IRequestHandler<GetDeliveryAsInvoiceQuery, DeliveryAsInvoiceDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDeliveryAsInvoiceQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DeliveryAsInvoiceDto?> Handle(GetDeliveryAsInvoiceQuery request, CancellationToken cancellationToken)
    {
        var delivery = await _context.Deliveries
                                .Include(t => t.DeliveryLines)!
                                .ThenInclude(l => l.Product)
                                .ThenInclude(p => p!.SellingPrices)
                                .Include(p => p!.SaleOrder)
                                .SingleOrDefaultAsync(t => t.Reference!.ToLower() == request.Term.ToLower());

        if (delivery == null)
            return null;

        var invoice = new DeliveryAsInvoiceDto();
        string? customerRef = string.Empty;
        if (delivery.CustomerId != null)
        {
            var customer = _context.Customers.Find(delivery.CustomerId);
            customerRef = $"{customer!.Reference} - {(customer!.ClientType == ClientType.Particular ? $"{customer!.FirstName}, {customer!.LastName}" : customer!.SocialReason)}";
        }

        invoice.Id = delivery.Id;
        invoice.Reference = delivery.Reference;
        invoice.CustomerId = delivery.CustomerId;
        invoice.CustomerRef = customerRef;
        invoice.SaleOrderId = delivery.SaleOrderId;
        invoice.SaleOrderRef = delivery.SaleOrder?.Reference;
        invoice.PaymentTerms = delivery.SaleOrder?.PaymentTerms;

        invoice.InvoiceLines = new List<InvoiceLineDto>();

        foreach (var saleLine in delivery.DeliveryLines!)
        {
            var line = new InvoiceLineDto
            {
                Id = Guid.NewGuid(),
                ProductId = saleLine.ProductId,
                DeliveryRef = delivery.Reference,
                SellingPriceId = saleLine.SellingPriceId,
                Reference = saleLine.Product!.Reference,
                ShortDesignation = saleLine.Product!.ShortDesignation,
                Qty = saleLine.Qty,
                ExpectedQty = saleLine.Qty,
                Price = saleLine.UnitPrice,
                Discount = saleLine.Discount ?? 0,
                VAT = saleLine.VAT ?? 0
            };


            line.TotalPriceExcludingTax = line.Qty * (line.Price - (line.Price * ((double)line.Discount / 100)));
            line.TotalVATPrice = line.TotalPriceExcludingTax * (((double?)line.VAT ?? 0) / 100);
            line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + line.TotalVATPrice;

            line.SellingPrices = _mapper.Map<List<SellingPriceDto>>(saleLine.Product.SellingPrices!.ToList());

            invoice.InvoiceLines.Add(line);
        }

        return invoice;
    }
}
