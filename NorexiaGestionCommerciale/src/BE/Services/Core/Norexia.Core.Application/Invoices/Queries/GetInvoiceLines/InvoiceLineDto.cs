using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Application.Products.Commands.CreateProduct;
using Norexia.Core.Domain.InvoiceEntities;

namespace Norexia.Core.Application.Invoices.Queries.GetInvoiceLines;
public class InvoiceLineDto : IMapFrom<InvoiceLine>
{
    public Guid Id { get; set; }
    public Guid? SellingPriceId { get; set; }
    public Guid? ProductId { get; set; }
    public string? Reference { get; set; }
    public string? ShortDesignation { get; set; }
    public string? DeliveryRef { get; set; }
    public int? Qty { get; set; }
    public int? ExpectedQty { get; set; }
    public double? Price { get; set; }
    public int? Discount { get; set; }
    public int? VAT { get; set; }
    public double? TotalPriceExcludingTax { get; set; }
    public double? TotalVATPrice { get; set; }
    public double? TotalPriceIncludingTax { get; set; }
    public ICollection<SellingPriceDto>? SellingPrices { get; set; }
}
