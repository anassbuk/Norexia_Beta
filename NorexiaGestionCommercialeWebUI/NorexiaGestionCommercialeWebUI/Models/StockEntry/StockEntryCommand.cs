using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.StockEntry;
public class StockEntryCommand
{
    public Guid? Id { get; set; }
    public string? Reference { get; set; }
    public Guid? ProviderId { get; set; }
    public string? ProviderRef { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public string? PurchaseOrderRef { get; set; }
    public StockEntryOrigin? StockEntryOrigin { get; set; }
    public DateTimeOffset? EntryDate { get; set; } = DateTime.Now;
    public string? Status { get; set; }
    public StockRecordType? Type { get; set; }
    public string? Note { get; set; }
    public string? CreatedBy { get; set; }
    public ICollection<StockEntryLineDto>? StockEntryLines { get; set; }
}
