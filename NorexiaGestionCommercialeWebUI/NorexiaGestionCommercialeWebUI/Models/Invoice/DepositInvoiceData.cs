namespace NorexiaGestionCommercialeWebUI.Models.Invoice;
public class DepositInvoiceData
{
    public string? Designation { get; set; }
    public int? Qty { get; set; }
    public double? Price { get; set; }
    public double? TotalPriceExcludingTax { get; set; }
    public double? TotalVATPrice { get; set; }
    public double? TotalPriceIncludingTax { get; set; }
}
