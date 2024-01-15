using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.StockEntities;

namespace Norexia.Core.Application.StockEntries.Queries.GetStockEntries;
public class StockEntryDto : IMapFrom<StockEntry>
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? ProviderId { get; set; }
    public string? ProviderRef { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public string? PurchaseOrderRef { get; set; }
    public StockEntryOrigin StockEntryOrigin { get; set; }
    public DateTime EntryDate { get; set; }
    public string? Status { get; set; }
    public StockRecordType? Type { get; set; }
    public string? Note { get; set; }
    public string? CreatedBy { get; set; }
}
