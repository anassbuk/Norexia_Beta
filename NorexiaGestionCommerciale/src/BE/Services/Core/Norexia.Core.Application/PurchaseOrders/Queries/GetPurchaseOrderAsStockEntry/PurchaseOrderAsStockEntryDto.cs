using Norexia.Core.Application.StockEntries.Queries.GetStockEntryLines;

namespace Norexia.Core.Application.PurchaseOrders.Queries.GetPurchaseOrderAsStockEntry;
public class PurchaseOrderAsStockEntryDto
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid ProviderId { get; set; }
    public string? ProviderRef { get; set; }
    public ICollection<StockEntryLineDto>? StockEntryLines { get; set; }
}
