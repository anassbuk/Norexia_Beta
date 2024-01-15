using MediatR;

using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.StockEntries.Commands.CreateStockEntry;
public class CreateStockEntryCommand : IRequest<Guid>
{
    public string? Reference { get; set; }
    public Guid? ProviderId { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public StockEntryOrigin StockEntryOrigin { get; set; }
    public DateTime EntryDate { get; set; }
    public string? Status { get; set; }
    public StockRecordType? Type { get; set; }
    public string? Note { get; set; }
    public virtual ICollection<StockEntryLineCommand>? StockEntryLines { get; set; }
}
