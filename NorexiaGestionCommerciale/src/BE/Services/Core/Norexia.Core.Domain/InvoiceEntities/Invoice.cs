using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.CustomerEntities;
using Norexia.Core.Domain.PaymentEntities;
using Norexia.Core.Domain.SaleOrderEntities;
using Norexia.Core.Domain.SettingEntities;

namespace Norexia.Core.Domain.InvoiceEntities;
public class Invoice : BaseAuditableEntity
{
    public string? Reference { get; set; }
    public Guid? CustomerId { get; set; }
    public virtual Customer? Customer { get; set; }
    public Guid? SaleOrderId { get; set; }
    public virtual SaleOrder? SaleOrder { get; set; }
    public string? DeliveryRef { get; set; }
    public Guid? CurrencyId { get; set; }
    public virtual Currency? Currency { get; set; }
    public float? Discount { get; set; }
    public InvoiceOrigin InvoiceOrigin { get; set; }
    public InvoiceType InvoiceType { get; set; }
    public required OwnedPaymentTerms PaymentTerms { get; set; }
    public DateTime EntryDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? DeliveryStartDate { get; set; }
    public DateTime? DeliveryEndDate { get; set; }
    public string? Status { get; set; }
    public string? DirectCreationReason { get; set; }
    public string? Note { get; set; }
    public virtual ICollection<InvoiceLine>? InvoiceLines { get; set; }
    public virtual ICollection<InvoicePayment>? InvoicePayments { get; set; }
}
