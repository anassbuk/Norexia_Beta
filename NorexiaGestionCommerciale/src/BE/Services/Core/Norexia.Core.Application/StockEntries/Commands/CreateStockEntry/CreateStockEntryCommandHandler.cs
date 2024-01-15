using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.StockEntities;

namespace Norexia.Core.Application.StockEntries.Commands.CreateStockEntry;
public class CreateStockEntryCommandHandler : IRequestHandler<CreateStockEntryCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateStockEntryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateStockEntryCommand request, CancellationToken cancellationToken)
    {
        if (await _context.StockEntries.AnyAsync(o => o.Reference == request.Reference))
            throw new NotFoundException($"Stock entry with reference ({request.Reference}) already exist! ");

        if (!await _context.Providers.AnyAsync(t => t.Id == request.ProviderId))
            throw new NotFoundException($"Provider with id ({request.ProviderId}) not found!");

        StockEntry stockEntry = new()
        {
            Id = Guid.NewGuid(),
            Reference = request.Reference,
            ProviderId = request.ProviderId,
            PurchaseOrderId = request.PurchaseOrderId,
            StockEntryOrigin = request.StockEntryOrigin,
            EntryDate = request.EntryDate.ToUniversalTime(),
            Status = request.Status,
            Type = request.Type,
            Note = request.Note,
        };

        stockEntry.StockEntryLines = await HandleStockEntryLines(request, cancellationToken);

        var result = await _context.StockEntries.AddAsync(stockEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }

    public async Task<List<StockEntryLine>> HandleStockEntryLines(CreateStockEntryCommand request, CancellationToken cancellationToken)
    {
        List<StockEntryLine> stockEntryLines = new();
        foreach (var line in request.StockEntryLines!)
        {

            if (!await _context.Products.AnyAsync(t => t.Id == line.ProductId))
                throw new NotFoundException($"Product with id ({line.ProductId}) not found!");

            stockEntryLines.Add(new StockEntryLine()
            {
                ProductId = line.ProductId,
                Qty = line.Qty,
            });
        }

        return stockEntryLines;
    }
}
