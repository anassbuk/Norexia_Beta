using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.DeliveryEntities;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.Deliveries.Commands.CreateDelivery;
public class CreateDeliveryCommandHandler : IRequestHandler<CreateDeliveryCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateDeliveryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateDeliveryCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Deliveries.AnyAsync(o => o.Reference == request.Reference))
            throw new NotFoundException($"Delivery with reference ({request.Reference}) already exist! ");

        if (!await _context.Deliverers.AnyAsync(c => c.Id == request.DelivererId, cancellationToken))
            throw new NotFoundException($"Deliverer with id ({request.DelivererId}) not found! ");

        Delivery delivery = new()
        {
            Id = Guid.NewGuid(),
            Reference = request.Reference,
            CustomerId = request.CustomerId,
            DelivererId = request.DelivererId,
            SaleOrderId = request.SaleOrderId,
            InvoiceId = request.InvoiceId,
            DeliveryOrigin = request.DeliveryOrigin,
            EntryDate = request.EntryDate!.ToUniversalTime(),
            DeliveryDate = request.DeliveryDate!.ToUniversalTime(),
            PlannedDate = request.PlannedDate!.ToUniversalTime(),
            DeliveryTime = request.DeliveryTime!.ToUniversalTime(),
            DeliveryMode = request.DeliveryMode,
            Status = request.Status,
            Note = request.Note,
        };

        delivery.DeliveryLines = await HandleDeliveryLines(request, cancellationToken);

        var result = await _context.Deliveries.AddAsync(delivery, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }


    public async Task<List<DeliveryLine>> HandleDeliveryLines(CreateDeliveryCommand request, CancellationToken cancellationToken)
    {
        List<DeliveryLine> deliveryLines = new();
        foreach (var line in request.DeliveryLines!)
        {
            SellingPrice? sellingPrice = await _context.SellingPrices.FindAsync(line.SellingPriceId, cancellationToken);

            if (sellingPrice is null)
                throw new NotFoundException($"Selling Price with id ({line.SellingPriceId}) not found! ");

            deliveryLines.Add(new DeliveryLine()
            {
                ProductId = sellingPrice.ProductId,
                SellingPriceId = line.SellingPriceId,
                UnitPrice = sellingPrice.Price,
                VAT = sellingPrice.VAT,
                Discount = sellingPrice.Discount,
                Qty = line.Qty,
                ExpectedQty = line.ExpectedQty,
            });
        }

        return deliveryLines;
    }
}
