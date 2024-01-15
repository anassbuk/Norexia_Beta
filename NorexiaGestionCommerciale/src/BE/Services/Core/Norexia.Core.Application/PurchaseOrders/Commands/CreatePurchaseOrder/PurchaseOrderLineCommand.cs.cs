namespace Norexia.Core.Application.PurchaseOrders.Commands.CreatePurchaseOrder;
public class PurchaseOrderLineCommand
{
    public Guid? Id { get; set; }
    public Guid PurchaseOrderId { get; set; }
    public Guid ProductId { get; set; }
    public double? Price { get; set; }
    public int? VAT { get; set; }
    public int? Qty { get; set; }
}
