using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.QuotationEntities;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.Quotations.Queries.GetQuotations;

public class QuotationDto : IMapFrom<Quotation>
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public DateTime? QuotationDate { get; set; }
    public int? ValidityDuretion { get; set; }
    public string? Responsable { get; set; }
    public Guid? CustomerId { get; set; }
    public string? Status { get; set; }
    public float? Discount { get; set; }
    public string? Note { get; set; }
    public int? Version { get; set; }
    public OwnedPaymentTerms? PaymentTerms { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DeliveryMode DeliveryMode { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public double? PriceExcludingTax { get; set; }
    public double? TaxPrice { get; set; }
    public double? PriceIncludingTax { get; set; }
    public double? DiscountPrice { get; set; }
    public double? NetPrice { get; set; }


}

