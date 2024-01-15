using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.PaymentEntities;
using Norexia.Core.Domain.ProductEntities;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.SaleOrders.Commands.UpdateSaleOrder;
public class UpdateSaleOrderCommandHandler : IRequestHandler<UpdateSaleOrderCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public UpdateSaleOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateSaleOrderCommand request, CancellationToken cancellationToken)
    {
        var saleOrder = await _context.SaleOrders
                                .Include(s => s.SaleOrderLines)
                                .SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (saleOrder == null)
            throw new NotFoundException($"Sale Order with id ({request.Id}) not found! ");

        if (request.CustomerId != null)
        {
            if (!await _context.Customers.AnyAsync(c => c.Id == request.CustomerId, cancellationToken))
                throw new NotFoundException($"Customer with id ({request.CustomerId}) not found! ");
        }

        saleOrder.SaleOrderOrigin = request.SaleOrderOrigin;
        saleOrder.QuotationId = request.QuotationId;
        saleOrder.OperationType = request.OperationType;
        saleOrder.Execution = request.Execution;
        saleOrder.CustomerId = request.CustomerId;
        saleOrder.Discount = request.Discount;
        saleOrder.OrderDate = request.OrderDate!.Value.ToUniversalTime();
        saleOrder.DeliveryDate = request.DeliveryDate?.ToUniversalTime();
        saleOrder.DeliveryMode = request.DeliveryMode;
        saleOrder.Note = request.Note;
        saleOrder.SaleChannelId = request.SaleChannelId;
        saleOrder.Status = request.Status;
        saleOrder.PaymentTerms = request.PaymentTerms;

        await HandleSaleOrderLines(request, saleOrder, cancellationToken);

        await HandleSaleOrderPayments(request, saleOrder, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return saleOrder.Id;
    }

    private async Task HandleSaleOrderLines(UpdateSaleOrderCommand request, SaleOrder saleOrder, CancellationToken cancellationToken)
    {
        var linesToRemove = saleOrder.SaleOrderLines!
            .Where(l => !request.SaleOrderLines!.Any(rl => rl.Id == l.Id));

        foreach (var line in linesToRemove.ToList())
            saleOrder.SaleOrderLines!.Remove(line);


        foreach (var item in request.SaleOrderLines!)
        {
            SellingPrice? sellingPrice = await _context.SellingPrices.FindAsync(item.SellingPriceId, cancellationToken);

            if (sellingPrice is null)
                throw new NotFoundException($"Selling Price with id ({request.CustomerId}) not found! ");

            if (saleOrder.SaleOrderLines!.Any(l => l.Id == item.Id))
            {
                var line = saleOrder.SaleOrderLines!.First(l => l.Id == item.Id);
                line.SellingPriceId = item.SellingPriceId;
                line.Qty = item.Qty;
                line.ProductId = sellingPrice.ProductId;
                line.Price = sellingPrice.Price;
                line.VAT = sellingPrice.VAT;
                line.Margin = sellingPrice.Margin;
                line.Discount = sellingPrice.Discount;
            }
            else
            {
                saleOrder.SaleOrderLines!.Add(new SaleOrderLine()
                {
                    Id = Guid.NewGuid(),
                    SellingPriceId = item.SellingPriceId,
                    Qty = item.Qty,
                    ProductId = sellingPrice.ProductId,
                    Price = sellingPrice.Price,
                    VAT = sellingPrice.VAT,
                    Margin = sellingPrice.Margin,
                    Discount = sellingPrice.Discount,
                });
            }
        }
    }


    public async Task HandleSaleOrderPayments(UpdateSaleOrderCommand request, SaleOrder saleOrder, CancellationToken cancellationToken)
    {
        var linesToRemove = saleOrder.SalePayments!
            .Where(l => !request.SalePayments!.Any(rl => rl.Id == l.Id));

        foreach (var line in linesToRemove.ToList())
            saleOrder.SalePayments!.Remove(line);

        foreach (var payment in request.SalePayments!)
        {
            if (!await _context.PaymentMeans.AnyAsync(c => c.Id == payment.PaymentMeanId, cancellationToken))
                throw new NotFoundException($"Payment mean with id ({payment.PaymentMeanId}) not found! ");

            if (saleOrder.SalePayments!.Any(l => l.Id == payment.Id))
            {
                var salePayment = saleOrder.SalePayments!.First(l => l.Id == payment.Id);

                salePayment.PaymentMeanId = payment.PaymentMeanId;
                salePayment.EntryDate = payment.EntryDate.ToUniversalTime();
                salePayment.DueDate = payment.DueDate?.ToUniversalTime();
                salePayment.OperationDate = payment.OperationDate?.ToUniversalTime();
                salePayment.Status = payment.Status;
                salePayment.Note = request.Note;
                salePayment.AmountToBePaid = payment.AmountToBePaid;
                salePayment.AmountPaid = payment.AmountPaid;
            }
            else
            {
                if (await _context.SalePayments.AnyAsync(o => o.Reference == payment.Reference))
                    throw new NotFoundException($"Payment with reference ({payment.Reference}) already exist! ");

                saleOrder.SalePayments!.Add(new SalePayment()
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
        }
    }
}
