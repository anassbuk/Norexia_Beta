using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.PurchaseOrders.Commands.DeletePurchaseOrder;
public record DeletePurchaseOrderCommand(IEnumerable<Guid> Ids) : IRequest;
public class DeletePurchaseOrderCommandHandler : IRequestHandler<DeletePurchaseOrderCommand>
{
    private readonly IApplicationDbContext _context;
    public DeletePurchaseOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeletePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var purchaseOrder = await _context.PurchaseOrders.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (purchaseOrder == null)
            {
                throw new NotFoundException($"Purchase Order with id ({id}) not found! ");
            }

            purchaseOrder.IsDeleted = true;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
