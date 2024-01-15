using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.PaymentTerms.Queries;
public class PaymentTermsDto : IMapFrom<Domain.SaleOrderEntities.PaymentTerms>
{
    public int? MaturityDuration { get; set; }
    public bool? MaturityDurationNegotiable { get; set; }
    public bool? DepositInvoice { get; set; }
    public bool? DepositInvoiceNegotiable { get; set; }
    public int? DepositInvoiceDownPayment { get; set; }
    public bool? PaymentByInstallments { get; set; }
    public bool? PaymentByInstallmentsNegotiable { get; set; }
    public int? PaymentByInstallmentsNumber { get; set; }
    public PaymentOption? PaymentOption { get; set; }
}
