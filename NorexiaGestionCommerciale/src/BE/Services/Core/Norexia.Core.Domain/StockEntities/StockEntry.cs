using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.ProviderEntities;
using Norexia.Core.Domain.PurchaseOrderEntities;

namespace Norexia.Core.Domain.StockEntities;
public class StockEntry : BaseAuditableEntity
{
    public string? Reference { get; set; }
    public Guid? ProviderId { get; set; }
    public virtual Provider? Provider { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public virtual PurchaseOrder? PurchaseOrder { get; set; }
    public StockEntryOrigin StockEntryOrigin { get; set; }
    public DateTime EntryDate { get; set; }
    public string? Status { get; set; }
    public StockRecordType? Type { get; set; }
    public string? Note { get; set; }
    public virtual ICollection<StockEntryLine>? StockEntryLines { get; set; }
}
