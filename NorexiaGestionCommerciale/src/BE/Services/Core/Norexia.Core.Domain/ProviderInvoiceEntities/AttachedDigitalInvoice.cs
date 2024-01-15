using Norexia.Core.Domain.Common;

namespace Norexia.Core.Domain.ProviderInvoiceEntities;
public class AttachedDigitalInvoice : BaseAuditableEntity
{
    public Guid InvoiceId { get; set; }
    public string? Label { get; set; }
    public string? Path { get; set; }
}
