using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.PaymentEntities;
using Norexia.Core.Domain.ProductEntities;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.SaleOrders.Commands.CreateSaleOrder;
public class CreateSaleOrderCommandHandler : IRequestHandler<CreateSaleOrderCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateSaleOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateSaleOrderCommand request, CancellationToken cancellationToken)
    {

        if (await _context.SaleOrders.AnyAsync(o => o.Reference == request.Reference))
            throw new NotFoundException($"Sale order with reference ({request.Reference}) already exist! ");

        if (request.CustomerId != null)
        {
            if (!await _context.Customers.AnyAsync(c => c.Id == request.CustomerId, cancellationToken))
                throw new NotFoundException($"Customer with id ({request.CustomerId}) not found! ");
        }

        SaleOrder saleOrder = new()
        {
            Id = Guid.NewGuid(),
            OperationType = request.OperationType,
            Execution = request.Execution,
            SaleOrderOrigin = request.SaleOrderOrigin,
            QuotationId = request.QuotationId,
            Reference = request.Reference,
            CustomerId = request.CustomerId,
            OrderDate = request.OrderDate!.Value.ToUniversalTime(),
            DeliveryDate = request.DeliveryDate!.Value.ToUniversalTime(),
            DeliveryMode = request.DeliveryMode,
            Discount = request.Discount,
            Note = request.Note,
            Status = request.Status,
            SaleChannelId = request.SaleChannelId,
            PaymentTerms = request.PaymentTerms,
        };

        saleOrder.SaleOrderLines = await HandleSaleOrderLines(request, cancellationToken);

        saleOrder.SalePayments = await HandleSaleOrderPayments(request, cancellationToken);

        var result = await _context.SaleOrders.AddAsync(saleOrder, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;

    }

    public async Task<List<SaleOrderLine>> HandleSaleOrderLines(CreateSaleOrderCommand request, CancellationToken cancellationToken)
    {
        List<SaleOrderLine> SaleOrderLines = new();
        foreach (var line in request.SaleOrderLines!)
        {
            SellingPrice? sellingPrice = await _context.SellingPrices.FindAsync(line.SellingPriceId, cancellationToken);

            if (sellingPrice is null)
                throw new NotFoundException($"Selling Price with id ({request.CustomerId}) not found! ");

            SaleOrderLines.Add(new SaleOrderLine()
            {
                Id = Guid.NewGuid(),
                ProductId = sellingPrice.ProductId,
                SellingPriceId = line.SellingPriceId,
                Price = sellingPrice.Price,
                VAT = sellingPrice.VAT,
                Margin = sellingPrice.Margin,
                Discount = sellingPrice.Discount,
                Qty = line.Qty,
            });
        }

        return SaleOrderLines;
    }

    public async Task<List<SalePayment>> HandleSaleOrderPayments(CreateSaleOrderCommand request, CancellationToken cancellationToken)
    {
        List<SalePayment> SalePayments = new();
        foreach (var payment in request.SalePayments!)
        {
            if (await _context.SalePayments.AnyAsync(o => o.Reference == payment.Reference))
                throw new NotFoundException($"Payment with reference ({payment.Reference}) already exist! ");

            if (!await _context.PaymentMeans.AnyAsync(c => c.Id == payment.PaymentMeanId, cancellationToken))
                throw new NotFoundException($"Payment mean with id ({payment.PaymentMeanId}) not found! ");

            SalePayments.Add(new SalePayment()
            {
                Id = Guid.NewGuid(),
                Reference = payment.Reference,
                PaymentMeanId = payment.PaymentMeanId,
                EntryDate = payment.EntryDate.ToUniversalTime(),
                DueDate = payment.DueDate?.ToUniversalTime(),
                OperationDate = payment.OperationDate?.ToUniversalTime(),
                Status = payment.Status,
                Note = request.Note,
                AmountToBePaid = payment.AmountToBePaid,
                AmountPaid = payment.AmountPaid,
            });
        }

        return SalePayments;
    }

}
