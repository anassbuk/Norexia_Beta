using Norexia.Core.Application.Invoices.Queries.GetInvoiceLines;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.Deliveries.Queries.GetDeliveryAsInvoice;
public class DeliveryAsInvoiceDto
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? SaleOrderId { get; set; }
    public string? SaleOrderRef { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerRef { get; set; }
    public virtual ICollection<InvoiceLineDto>? InvoiceLines { get; set; }
    public OwnedPaymentTerms? PaymentTerms { get; set; }
}
