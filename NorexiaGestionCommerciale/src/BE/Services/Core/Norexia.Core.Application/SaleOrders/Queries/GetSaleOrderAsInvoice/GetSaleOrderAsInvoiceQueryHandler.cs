using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Invoices.Queries.GetInvoiceLines;
using Norexia.Core.Application.Payments.Queries.GetPayments;
using Norexia.Core.Application.Products.Commands.CreateProduct;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.SaleOrders.Queries.GetSaleOrderAsInvoice;
public record GetSaleOrderAsInvoiceQuery(string Term) : IRequest<SaleOrderAsInvoiceDto?>;
public class GetSaleOrderAsInvoiceQueryHandler : IRequestHandler<GetSaleOrderAsInvoiceQuery, SaleOrderAsInvoiceDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSaleOrderAsInvoiceQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SaleOrderAsInvoiceDto?> Handle(GetSaleOrderAsInvoiceQuery request, CancellationToken cancellationToken)
    {
        var sale = await _context.SaleOrders
                                .Include(t => t.SalePayments)!
                                .Include(t => t.SaleOrderLines)!
                                .ThenInclude(l => l.Product)
                                .ThenInclude(p => p!.SellingPrices)
                                .SingleOrDefaultAsync(t => t.Reference!.ToLower() == request.Term.ToLower());

        if (sale == null)
            return null;

        var invoice = new SaleOrderAsInvoiceDto();

        invoice.InvoicePayments = _mapper.Map<List<PaymentDto>>(sale.SalePayments);

        string customerRef = string.Empty;
        if (sale.CustomerId != null)
        {
            var customer = _context.Customers.Find(sale.CustomerId);
            customerRef = $"{customer!.Reference} - {(customer!.ClientType == ClientType.Particular ? $"{customer!.FirstName}, {customer!.LastName}" : customer!.SocialReason)}";
        }

        invoice.Id = sale.Id;
        invoice.Reference = sale.Reference;
        invoice.CustomerId = sale.CustomerId;
        invoice.CustomerRef = customerRef;
        invoice.PaymentTerms = sale.PaymentTerms;

        invoice.InvoiceLines = new List<InvoiceLineDto>();

        foreach (var saleLine in sale.SaleOrderLines!)
        {
            var line = new InvoiceLineDto();

            line.Id = Guid.NewGuid();
            line.ProductId = saleLine.ProductId;
            line.SellingPriceId = saleLine.SellingPriceId;
            line.Reference = saleLine.Product!.Reference;
            line.ShortDesignation = saleLine.Product!.ShortDesignation;
            line.Qty = saleLine.Qty;
            line.ExpectedQty = saleLine.Qty;
            line.Price = saleLine.Price;
            line.Discount = saleLine.Discount ?? 0;
            line.VAT = saleLine.VAT ?? 0;


            line.TotalPriceExcludingTax = line.Qty * (line.Price - (line.Price * ((double)line.Discount / 100)));
            line.TotalVATPrice = line.TotalPriceExcludingTax * (((double?)line.VAT ?? 0) / 100);
            line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + line.TotalVATPrice;

            line.SellingPrices = _mapper.Map<List<SellingPriceDto>>(saleLine.Product.SellingPrices!.ToList());

            invoice.InvoiceLines.Add(line);
        }

        return invoice;
    }
}
