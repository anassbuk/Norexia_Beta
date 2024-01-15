namespace Norexia.Core.Application.SaleOrders.Commands.CreateSaleOrder;
public class SaleOrderLineCommand
{
    public Guid? Id { get; set; }
    public Guid? SellingPriceId { get; set; }
    public int? Qty { get; set; }
}
