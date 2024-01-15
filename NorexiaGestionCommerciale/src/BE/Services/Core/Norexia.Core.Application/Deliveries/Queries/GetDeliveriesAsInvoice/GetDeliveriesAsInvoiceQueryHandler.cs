using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Invoices.Queries.GetInvoiceLines;
using Norexia.Core.Application.Products.Commands.CreateProduct;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.Deliveries.Queries.GetDeliveriesAsInvoice;
public record GetDeliveriesAsInvoiceQuery(string Term, DateTime DeliveryStartDate, DateTime DeliveryEndDate) : IRequest<DeliveriesAsInvoiceDto?>;
public class GetDeliveriesAsInvoiceQueryHandler : IRequestHandler<GetDeliveriesAsInvoiceQuery, DeliveriesAsInvoiceDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDeliveriesAsInvoiceQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DeliveriesAsInvoiceDto?> Handle(GetDeliveriesAsInvoiceQuery request, CancellationToken cancellationToken)
    {
        var sale = await _context.SaleOrders
                            .FirstOrDefaultAsync(s => s.Reference!.ToLower() == request.Term.ToLower());

        if (sale == null)
            return null;

        if (!await _context.Deliveries.AnyAsync(d => d.SaleOrderId == sale.Id))
            return null;

        var invoice = new DeliveriesAsInvoiceDto();
        string? customerRef = string.Empty;
        if (sale.CustomerId != null)
        {
            var customer = _context.Customers.Find(sale.CustomerId);
            customerRef = $"{customer!.Reference} - {(customer!.ClientType == ClientType.Particular ? $"{customer!.FirstName}, {customer!.LastName}" : customer!.SocialReason)}";
        }

        invoice.SaleOrderId = sale.Id;
        invoice.SaleOrderRef = sale.Reference;
        invoice.CustomerId = sale.CustomerId;
        invoice.PaymentTerms = sale.PaymentTerms;
        invoice.CustomerRef = customerRef;

        var deliveries = await _context.Deliveries
                                .Include(t => t.DeliveryLines)!
                                .ThenInclude(l => l.Product)
                                .ThenInclude(p => p!.SellingPrices)
                                .Include(p => p!.SaleOrder)
                                .Where(t => t.SaleOrderId == sale.Id && (t.DeliveryDate > request.DeliveryStartDate.ToUniversalTime() && t.DeliveryDate < request.DeliveryEndDate.ToUniversalTime()))
                                .ToListAsync();

        if (!deliveries.Any())
            return null;

        invoice.InvoiceLines = new List<InvoiceLineDto>();

        foreach (var delivery in deliveries)
        {
            foreach (var saleLine in delivery.DeliveryLines!)
            {
                var line = new InvoiceLineDto
                {
                    Id = Guid.NewGuid(),
                    ProductId = saleLine.ProductId,
                    SellingPriceId = saleLine.SellingPriceId,
                    Reference = saleLine.Product!.Reference,
                    DeliveryRef = delivery.Reference,
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
        }

        return invoice;
    }
}
