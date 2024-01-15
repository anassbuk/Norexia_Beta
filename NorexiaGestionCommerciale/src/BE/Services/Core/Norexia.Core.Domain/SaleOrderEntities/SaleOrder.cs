using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.CustomerEntities;
using Norexia.Core.Domain.PaymentEntities;
using Norexia.Core.Domain.ProductEntities;
using Norexia.Core.Domain.QuotationEntities;

namespace Norexia.Core.Domain.SaleOrderEntities;
public class SaleOrder : BaseAuditableEntity
{
    public SaleOperationType OperationType { get; set; }
    public SaleExecution Execution { get; set; }
    public string? Reference { get; set; }
    public Guid? CustomerId { get; set; }
    public virtual Customer? Customer { get; set; }
    public SaleOrderOrigin SaleOrderOrigin { get; set; }
    public Guid? QuotationId { get; set; }
    public virtual Quotation? Quotation { get; set; }
    public float? Discount { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime OrderDate { get; set; }
    public string? Status { get; set; }
    public string? Note { get; set; }
    public DeliveryMode DeliveryMode { get; set; }
    public virtual ICollection<SaleOrderLine>? SaleOrderLines { get; set; }
    public Guid? SaleChannelId { get; set; }
    public virtual ProductAvailability? SaleChannel { get; set; }
    public required OwnedPaymentTerms PaymentTerms { get; set; }
    public virtual ICollection<SalePayment>? SalePayments { get; set; }
}
