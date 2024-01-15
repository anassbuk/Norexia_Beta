namespace Norexia.Core.Application.StockEntries.Commands.CreateStockEntry;
public class StockEntryLineCommand
{
    public Guid? Id { get; set; }
    public Guid? ProductId { get; set; }
    public int? Qty { get; set; }
}
