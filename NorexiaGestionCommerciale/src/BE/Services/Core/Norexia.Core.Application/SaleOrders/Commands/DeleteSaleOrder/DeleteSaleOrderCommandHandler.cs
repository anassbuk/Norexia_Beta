using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.SaleOrders.Commands.DeleteSaleOrder;
internal class DeleteSaleOrderCommandHandler : IRequestHandler<DeleteSaleOrderCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteSaleOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSaleOrderCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var saleOrder = await _context.SaleOrders.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (saleOrder == null)
            {
                throw new NotFoundException($"Sale Order with id ({id}) not found! ");
            }

            saleOrder.IsDeleted = true;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
public record DeleteSaleOrderCommand(IEnumerable<Guid> Ids) : IRequest;
