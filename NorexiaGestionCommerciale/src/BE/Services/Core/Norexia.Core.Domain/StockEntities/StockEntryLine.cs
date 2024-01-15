using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Domain.StockEntities;
public class StockEntryLine : BaseAuditableEntity
{
    public Guid? ProductId { get; set; }
    public virtual Product? Product { get; set; }
    public int? Qty { get; set; }
}
