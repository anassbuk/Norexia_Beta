using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.PurchaseOrderEntities;

namespace Norexia.Core.Application.PurchaseOrders.Commands.UpdatePurchaseOrder;
public class UpdatePurchaseOrderCommandHandler : IRequestHandler<UpdatePurchaseOrderCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public UpdatePurchaseOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _context.PurchaseOrders
                                .Include(s => s.PurchaseOrderLines)
                                .SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (purchaseOrder == null)
            throw new NotFoundException($"Purchase Order with id ({request.Id}) not found! ");

        purchaseOrder.ProviderId = request.ProviderId;
        purchaseOrder.DeliveryDate = request.DeliveryDate;
        purchaseOrder.Status = request.Status;
        purchaseOrder.OrderDate = request.OrderDate!.Value.ToUniversalTime();
        purchaseOrder.Note = request.Note;

        HandlePurchaseOrderLines(request, purchaseOrder, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return purchaseOrder.Id;
    }

    private void HandlePurchaseOrderLines(UpdatePurchaseOrderCommand request, PurchaseOrder purchaseOrder, CancellationToken cancellationToken)
    {
        var linesToRemove = purchaseOrder.PurchaseOrderLines!
            .Where(l => !request.PurchaseOrderLines!.Any(rl => rl.Id == l.Id));

        foreach (var line in linesToRemove.ToList())
            purchaseOrder.PurchaseOrderLines!.Remove(line);


        foreach (var item in request.PurchaseOrderLines!)
        {
            if (purchaseOrder.PurchaseOrderLines!.Any(l => l.Id == item.Id))
            {
                var line = purchaseOrder.PurchaseOrderLines!.First(l => l.Id == item.Id);
                line.Qty = item.Qty;
                line.Price = item.Price;
                line.VAT = item.VAT;
            }
            else
            {
                purchaseOrder.PurchaseOrderLines!.Add(new PurchaseOrderLine()
                {
                    Qty = item.Qty,
                    Price = item.Price,
                    VAT = item.VAT,
                });
            }
        }
    }
}