using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.CustomerEntities;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Domain.QuotationEntities;

public class Quotation : BaseAuditableEntity
{
    public string? Reference { get; set; }
    public DateTime? QuotationDate { get; set; }
    public int? ValidityDuration { get; set; }
    public String? Responsable { get; set; }
    public Guid? CustomerId { get; set; }
    public virtual Customer? Customer { get; set; }
    public String? Status { get; set; }
    public float? Discount { get; set; }
    public string? Note { get; set; }
    public int? Version { get; set; }
    public DateTime? OrderDate { get; set; }
    public required OwnedPaymentTerms PaymentTerms { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DeliveryMode? DeliveryMode { get; set; }
    public virtual ICollection<QuotationLine>? QuotationLines { get; set; }

}
