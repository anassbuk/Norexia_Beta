using Microsoft.EntityFrameworkCore;

namespace Norexia.Core.Domain.SaleOrderEntities;

[Owned]
public class OwnedPaymentTerms
{
    public int? MaturityDuration { get; set; }
    public bool? DepositInvoice { get; set; }
    public int? DepositInvoiceDownPayment { get; set; }
    public bool? PaymentByInstallments { get; set; }
    public int? PaymentByInstallmentsNumber { get; set; }
}
