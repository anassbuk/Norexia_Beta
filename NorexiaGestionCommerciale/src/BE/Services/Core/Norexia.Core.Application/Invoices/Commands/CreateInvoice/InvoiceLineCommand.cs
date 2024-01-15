namespace Norexia.Core.Application.Invoices.Commands.CreateInvoice;

public class InvoiceLineCommand
{
    public Guid? Id { get; set; }
    public Guid? SellingPriceId { get; set; }
    public int? Qty { get; set; }
    public int? ExpectedQty { get; set; }
    public string? DeliveryRef { get; set; }
}
