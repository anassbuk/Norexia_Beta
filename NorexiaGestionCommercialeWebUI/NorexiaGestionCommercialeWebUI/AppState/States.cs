using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models;

namespace NorexiaGestionCommercialeWebUI.AppState;
public class States
{
    public SaleOrderDto? SaleOrder { get; set; }
    public PurchaseOrderDto? PurchaseOrder { get; set; }
    public StockEntryDto? StockEntry { get; set; }
    public DelivererDto? Deliverer { get; set; }
    public DeliveryDto? Delivery { get; set; }
    public InvoiceDto? Invoice { get; set; }
    public QuotationDto? Quotation { get; set; }
    public PaymentDto? Payment { get; set; }
    public CreditNoteDto? CreditNote { get; set; }
}
