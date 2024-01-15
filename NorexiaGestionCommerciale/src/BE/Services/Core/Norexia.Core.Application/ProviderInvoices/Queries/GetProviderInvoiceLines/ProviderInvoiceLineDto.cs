using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.ProviderInvoiceEntities;

namespace Norexia.Core.Application.ProviderInvoices.Queries.GetProviderInvoiceLines;
public class ProviderInvoiceLineDto : IMapFrom<ProviderInvoiceLine>
{
    public Guid Id { get; set; }
    public Guid? ProductId { get; set; }
    public string? Reference { get; set; }
    public string? ShortDesignation { get; set; }
    public double? Price { get; set; }
    public int? VAT { get; set; }
    public int? Qty { get; set; }
    public int? ExpectedQty { get; set; }
    public double? TotalPriceExcludingTax { get; set; }
    public double? TotalVATPrice { get; set; }
    public double? TotalPriceIncludingTax { get; set; }
}
