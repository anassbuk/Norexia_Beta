using Norexia.Core.Application.Invoices.Queries.GetInvoiceLines;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.Deliveries.Queries.GetDeliveriesAsInvoice;

public class DeliveriesAsInvoiceDto
{
    public Guid? SaleOrderId { get; set; }
    public string? SaleOrderRef { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerRef { get; set; }
    public virtual ICollection<InvoiceLineDto>? InvoiceLines { get; set; }
    public OwnedPaymentTerms? PaymentTerms { get; set; }
}
