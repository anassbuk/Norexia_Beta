using Norexia.Core.Application.ProviderInvoices.Queries.GetProviderInvoiceLines;

namespace Norexia.Core.Application.ProviderInvoices.Queries.GetPurchaseOrderAsProviderInvoice;
public class PurchaseOrderAsProviderInvoiceDto
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid ProviderId { get; set; }
    public string? ProviderRef { get; set; }
    public ICollection<ProviderInvoiceLineDto>? ProviderInvoiceLines { get; set; }
}
