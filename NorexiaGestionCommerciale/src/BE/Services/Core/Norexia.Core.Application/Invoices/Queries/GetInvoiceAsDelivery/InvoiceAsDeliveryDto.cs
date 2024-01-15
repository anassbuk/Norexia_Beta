using Norexia.Core.Application.Deliveries.Queries.GetDeliveryLines;

namespace Norexia.Core.Application.Invoices.Queries.GetInvoiceAsDelivery;
public class InvoiceAsDeliveryDto
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerRef { get; set; }
    public virtual ICollection<DeliveryLineDto>? DeliveryLines { get; set; }
}
