using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.DeliveryEntities;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.Deliveries.Commands.UpdateDelivery;
public class UpdateDeliveryCommandHandler : IRequestHandler<UpdateDeliveryCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public UpdateDeliveryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateDeliveryCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.Deliverers.AnyAsync(c => c.Id == request.DelivererId, cancellationToken))
            throw new NotFoundException($"Deliverer with id ({request.DelivererId}) not found! ");

        var delivery = await _context.Deliveries
                                .Include(s => s.DeliveryLines)
                                .SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (delivery == null)
            throw new NotFoundException($"Delivery with id ({request.Id}) not found! ");


        delivery.CustomerId = request.CustomerId;
        delivery.DelivererId = request.DelivererId;
        delivery.SaleOrderId = request.SaleOrderId;
        delivery.InvoiceId = request.InvoiceId;
        delivery.DeliveryOrigin = request.DeliveryOrigin;
        delivery.EntryDate = request.EntryDate!.ToUniversalTime();
        delivery.DeliveryDate = request.DeliveryDate!.ToUniversalTime();
        delivery.PlannedDate = request.PlannedDate!.ToUniversalTime();
        delivery.DeliveryTime = request.DeliveryTime!.ToUniversalTime();
        delivery.DeliveryMode = request.DeliveryMode;
        delivery.Status = request.Status;
        delivery.Note = request.Note;

        await HandleDeliveryLines(request, delivery, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return delivery.Id;
    }

    public async Task HandleDeliveryLines(UpdateDeliveryCommand request, Delivery delivery, CancellationToken cancellationToken)
    {
        var linesToRemove = delivery.DeliveryLines!
            .Where(l => !request.DeliveryLines!.Any(rl => rl.Id == l.Id));

        foreach (var line in linesToRemove.ToList())
            delivery.DeliveryLines!.Remove(line);

        foreach (var line in request.DeliveryLines!)
        {
            SellingPrice? sellingPrice = await _context.SellingPrices.FindAsync(line.SellingPriceId, cancellationToken);

            if (sellingPrice is null)
                throw new NotFoundException($"Selling Price with id ({request.CustomerId}) not found! ");

            if (delivery.DeliveryLines!.Any(l => l.Id == line.Id))
            {
                var item = delivery.DeliveryLines!.First(l => l.Id == line.Id);
                item.ProductId = sellingPrice.ProductId;
                item.SellingPriceId = line.SellingPriceId;
                item.UnitPrice = sellingPrice.Price;
                item.VAT = sellingPrice.VAT;
                item.Discount = sellingPrice.Discount;
                item.Qty = line.Qty;
                item.ExpectedQty = line.ExpectedQty;
            }
            else
            {
                delivery.DeliveryLines!.Add(new DeliveryLine()
                {
                    ProductId = sellingPrice.ProductId,
                    SellingPriceId = line.SellingPriceId,
                    UnitPrice = sellingPrice.Price,
                    VAT = sellingPrice.VAT,
                    Discount = sellingPrice.Discount,
                    Qty = line.Qty,
                    ExpectedQty = line.ExpectedQty
                });
            }
        }
    }
}
