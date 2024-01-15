using MediatR;

using Norexia.Core.Application.StockEntries.Commands.CreateStockEntry;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.StockEntries.Commands.UpdateStockEntry;
public class UpdateStockEntryCommand : IRequest<Guid>
{
    public Guid? Id { get; set; }
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
