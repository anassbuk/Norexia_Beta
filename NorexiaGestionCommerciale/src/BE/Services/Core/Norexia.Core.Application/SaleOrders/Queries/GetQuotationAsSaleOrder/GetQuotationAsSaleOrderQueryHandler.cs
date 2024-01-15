using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Customers.Queries.GetCustomer;
using Norexia.Core.Application.Products.Commands.CreateProduct;
using Norexia.Core.Application.SaleOrders.Queries.GetSaleOrders;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.SaleOrders.Queries.GetQuotationAsSaleOrder;
public record GetQuotationAsSaleOrderQuery(string Term) : IRequest<QuotationAsSaleOrderDto?>;
public class GetQuotationAsSaleOrderQueryHandler : IRequestHandler<GetQuotationAsSaleOrderQuery, QuotationAsSaleOrderDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetQuotationAsSaleOrderQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<QuotationAsSaleOrderDto?> Handle(GetQuotationAsSaleOrderQuery request, CancellationToken cancellationToken)
    {
        var quotation = await _context.Quotations
                                .Include(t => t.Customer)!
                                .ThenInclude(t => t!.CustomerAddresses)
                                .Include(t => t.PaymentTerms)!
                                .Include(t => t.QuotationLines)!
                                .ThenInclude(l => l.Product)
                                .ThenInclude(p => p!.SellingPrices)
                                .SingleOrDefaultAsync(t => t.Reference!.ToLower() == request.Term.ToLower());

        if (quotation == null)
            return null;

        var sale = new QuotationAsSaleOrderDto();

        sale.Id = quotation.Id;
        sale.Reference = quotation.Reference;
        sale.CustomerId = quotation.CustomerId;
        sale.Discount = quotation.Discount;
        sale.PaymentTerms = quotation.PaymentTerms;
        sale.DeliveryDate = quotation.DeliveryDate;
        sale.DeliveryMode = quotation.DeliveryMode;

        if (quotation.Customer != null)
        {
            var customer = _context.Customers.Find(sale.CustomerId);
            sale.CustomerRef = $"{customer!.Reference} - {(customer!.ClientType == ClientType.Particular ? $"{customer!.FirstName}, {customer!.LastName}" : customer!.SocialReason)}";

            sale.BillingCustomerAddress = _mapper.Map<CustomerAddressDto>(customer.CustomerAddresses.FirstOrDefault(a => a.AddressType != AddressType.Delivery && a.Active == true));
            sale.DeliveryCustomerAddress = _mapper.Map<CustomerAddressDto>(customer.CustomerAddresses.FirstOrDefault(a => a.AddressType != AddressType.Billing && a.Active == true));
        }

        sale.SaleOrderLines = new List<SaleOrderLineDto>();

        foreach (var quotationLine in quotation.QuotationLines!)
        {
            var line = new SaleOrderLineDto();

            line.Id = Guid.NewGuid();
            line.ProductId = quotationLine.ProductId;
            line.SellingPriceId = quotationLine.SellingPriceId;
            line.Reference = quotationLine.Product!.Reference;
            line.ShortDesignation = quotationLine.Product!.ShortDesignation;
            line.Qty = quotationLine.Qty;
            line.Price = quotationLine.Price;
            line.Discount = quotationLine.Discount ?? 0;
            line.VAT = quotationLine.VAT ?? 0;

            line.TotalPriceExcludingTax = line.Qty * (line.Price - (line.Price * ((double)line.Discount / 100)));
            var totalVATPrice = line.TotalPriceExcludingTax * ((double)line.VAT! / 100);
            line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + totalVATPrice;

            line.SellingPrices = _mapper.Map<List<SellingPriceDto>>(quotationLine.Product.SellingPrices!.ToList());

            sale.SaleOrderLines.Add(line);
        }

        return sale;
    }
}
