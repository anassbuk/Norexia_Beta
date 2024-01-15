using Norexia.Core.Application.Invoices.Queries.GetInvoiceLines;
using Norexia.Core.Application.Payments.Queries.GetPayments;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.SaleOrders.Queries.GetSaleOrderAsInvoice;

public class SaleOrderAsInvoiceDto
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerRef { get; set; }
    public virtual ICollection<InvoiceLineDto>? InvoiceLines { get; set; }
    public OwnedPaymentTerms? PaymentTerms { get; set; }
    public virtual ICollection<PaymentDto>? InvoicePayments { get; set; }
}
