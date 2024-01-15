using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.PurchaseOrderEntities;

namespace Norexia.Core.Application.PurchaseOrders.Commands.CreatePurchaseOrder;
public class CreatePurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreatePurchaseOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        if (await _context.PurchaseOrders.AnyAsync(o => o.Reference == request.Reference))
            throw new NotFoundException($"Purchase order with reference ({request.Reference}) already exist! ");

        if (!await _context.Providers.AnyAsync(c => c.Id == request.ProviderId, cancellationToken))
            throw new NotFoundException($"Provider with id ({request.ProviderId}) not found! ");

        var purchaseOrder = new PurchaseOrder()
        {
            Id = Guid.NewGuid(),
            ProviderId = request.ProviderId,
            DeliveryDate = request.DeliveryDate,
            Status = request.Status,
            Reference = request.Reference,
            OrderDate = request.OrderDate!.Value.ToUniversalTime(),
            Note = request.Note,
        };

        purchaseOrder.PurchaseOrderLines = await HandlePurchaseOrderLines(request, cancellationToken);

        var result = await _context.PurchaseOrders.AddAsync(purchaseOrder, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }


    public async Task<List<PurchaseOrderLine>> HandlePurchaseOrderLines(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var PurchaseOrderLines = new List<PurchaseOrderLine>();
        foreach (var line in request.PurchaseOrderLines!)
        {
            if (!await _context.Products.AnyAsync(p => p.Id == line.ProductId))
                throw new NotFoundException($"Product with id ({line.ProductId}) not found! ");

            PurchaseOrderLines.Add(new PurchaseOrderLine()
            {
                ProductId = line.ProductId,
                Price = line.Price,
                VAT = line.VAT,
                Qty = line.Qty,
            });
        }

        return PurchaseOrderLines;
    }
}
