using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.ProviderInvoiceEntities;

namespace Norexia.Core.Application.ProviderPayments.Queries.GetProviderInvoiceAsPayment;
public class ProviderInvoiceAsPaymentDto : IMapFrom<ProviderInvoice>
{
    public Guid? Id { get; set; }
    public string? Reference { get; set; }
    public DateTime? EntryDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Status { get; set; }
    public double? SettledAmount { get; set; }
    public double? RemainingAmount { get; set; }
    public double? TotalPriceIncludingVAT { get; set; }
}
