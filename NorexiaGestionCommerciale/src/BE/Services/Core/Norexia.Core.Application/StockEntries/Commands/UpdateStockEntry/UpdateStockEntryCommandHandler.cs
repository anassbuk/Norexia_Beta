using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.StockEntities;

namespace Norexia.Core.Application.StockEntries.Commands.UpdateStockEntry;
public class UpdateStockEntryCommandHandler : IRequestHandler<UpdateStockEntryCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public UpdateStockEntryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateStockEntryCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.Providers.AnyAsync(t => t.Id == request.ProviderId))
            throw new NotFoundException($"Provider with id ({request.ProviderId}) not found!");

        var stockEntry = await _context.StockEntries
                                .Include(s => s.StockEntryLines)
                                .SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (stockEntry == null)
            throw new NotFoundException($"Stock entry with id ({request.Id}) not found! ");

        stockEntry.ProviderId = request.ProviderId;
        stockEntry.PurchaseOrderId = request.PurchaseOrderId;
        stockEntry.StockEntryOrigin = request.StockEntryOrigin;
        stockEntry.EntryDate = request.EntryDate.ToUniversalTime();
        stockEntry.Status = request.Status;
        stockEntry.Type = request.Type;
        stockEntry.Note = request.Note;

        await HandleStockEntryLines(request, stockEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return stockEntry.Id;
    }

    public async Task HandleStockEntryLines(UpdateStockEntryCommand request, StockEntry stockEntry, CancellationToken cancellationToken)
    {
        var linesToRemove = stockEntry.StockEntryLines!
            .Where(l => !request.StockEntryLines!.Any(rl => rl.Id == l.Id));

        foreach (var line in linesToRemove.ToList())
            stockEntry.StockEntryLines!.Remove(line);

        foreach (var line in request.StockEntryLines!)
        {
            if (!await _context.Products.AnyAsync(t => t.Id == line.ProductId))
                throw new NotFoundException($"Product with id ({line.ProductId}) not found!");

            if (stockEntry.StockEntryLines!.Any(l => l.Id == line.Id))
            {
                var item = stockEntry.StockEntryLines!.First(l => l.Id == line.Id);
                item.ProductId = line.ProductId;
                item.Qty = line.Qty;
            }
            else
            {
                stockEntry.StockEntryLines!.Add(new StockEntryLine()
                {
                    ProductId = line.ProductId,
                    Qty = line.Qty,
                });
            }
        }
    }
}
