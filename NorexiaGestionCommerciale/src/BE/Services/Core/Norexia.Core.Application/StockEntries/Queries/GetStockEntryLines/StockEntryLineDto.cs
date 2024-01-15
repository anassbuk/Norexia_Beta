using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.StockEntities;

namespace Norexia.Core.Application.StockEntries.Queries.GetStockEntryLines;
public class StockEntryLineDto : IMapFrom<StockEntryLine>
{
    public Guid Id { get; set; }
    public Guid? ProductId { get; set; }
    public string? Reference { get; set; }
    public string? ShortDesignation { get; set; }
    public int? ExpectedQty { get; set; }
    public int? Qty { get; set; }
}
