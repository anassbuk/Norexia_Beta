namespace Norexia.Core.Application.Deliveries.Commands.CreateDelivery;
public class DeliveryLineCommand
{
    public Guid? Id { get; set; }
    public Guid? SellingPriceId { get; set; }
    public int? Qty { get; set; }
    public int? ExpectedQty { get; set; }
}
