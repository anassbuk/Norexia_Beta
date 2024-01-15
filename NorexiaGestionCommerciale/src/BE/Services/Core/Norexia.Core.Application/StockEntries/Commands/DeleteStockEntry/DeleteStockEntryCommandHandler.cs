using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.StockEntries.Commands.DeleteStockEntry;
public record DeleteStockEntryCommand(IEnumerable<Guid> Ids) : IRequest;
public class DeleteStockEntryCommandHandler : IRequestHandler<DeleteStockEntryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteStockEntryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteStockEntryCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var stockEntry = await _context.StockEntries.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (stockEntry == null)
            {
                throw new NotFoundException($"Stock entry with id ({id}) not found!");
            }

            stockEntry.IsDeleted = true;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
