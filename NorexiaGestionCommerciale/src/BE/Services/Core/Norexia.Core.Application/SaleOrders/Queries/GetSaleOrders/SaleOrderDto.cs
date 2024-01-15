using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Application.Products.Commands.CreateProduct;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.SaleOrders.Queries.GetSaleOrders;
public class SaleOrderDto : IMapFrom<SaleOrder>
{
    public Guid Id { get; set; }
    public SaleOrderOrigin SaleOrderOrigin { get; set; }
    public SaleOperationType OperationType { get; set; }
    public SaleExecution Execution { get; set; }
    public string? Reference { get; set; }
    public Guid? QuotationId { get; set; }
    public string? QuotationRef { get; set; }
    public Guid? CustomerId { get; set; }
    public float? Discount { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? Status { get; set; }
    public string? Note { get; set; }
    public DeliveryMode DeliveryMode { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public double? PriceExcludingTax { get; set; }
    public double? TaxPrice { get; set; }
    public double? PriceIncludingTax { get; set; }
    public double? DiscountPrice { get; set; }
    public double? NetPrice { get; set; }
    public Guid? SaleChannelId { get; set; }
    public ICollection<ProductAssignedAvailabilityDto>? SaleChannels { get; set; }
    public OwnedPaymentTerms? PaymentTerms { get; set; }
}
