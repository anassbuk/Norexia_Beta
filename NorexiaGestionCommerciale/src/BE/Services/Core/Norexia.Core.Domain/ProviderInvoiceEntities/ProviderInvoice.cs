using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.PaymentEntities;
using Norexia.Core.Domain.ProviderEntities;
using Norexia.Core.Domain.PurchaseOrderEntities;
using Norexia.Core.Domain.SaleOrderEntities;
using Norexia.Core.Domain.SettingEntities;

namespace Norexia.Core.Domain.ProviderInvoiceEntities;
public class ProviderInvoice : BaseAuditableEntity
{
    public string? Reference { get; set; }
    public Guid? ProviderId { get; set; }
    public virtual Provider? Provider { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public virtual PurchaseOrder? PurchaseOrder { get; set; }
    public Guid? CurrencyId { get; set; }
    public virtual Currency? Currency { get; set; }
    public float? Discount { get; set; }
    public ProviderInvoiceOrigin ProviderInvoiceOrigin { get; set; }
    public DateTime EntryDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Status { get; set; }
    public string? DirectCreationReason { get; set; }
    public string? Note { get; set; }
    public OwnedPaymentTerms? PaymentTerms { get; set; }
    public virtual ICollection<ProviderInvoiceLine>? ProviderInvoiceLines { get; set; }
    public virtual ICollection<AttachedDigitalInvoice>? AttachedDigitalInvoices { get; set; }
    public virtual ICollection<ProviderInvoicePayment>? ProviderInvoicePayments { get; set; }
}
