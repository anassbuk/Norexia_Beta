using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.SaleOrderEntities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.Payments.Queries.GetSaleOrderAsPayment;
public class SaleOrderAsPaymentDto : IMapFrom<SaleOrder>
{
    public Guid? Id { get; set; }
    public string? Reference { get; set; }
    public DateTime? EntryDate { get; set; }
    public string? Status { get; set; }
    public double? SettledAmount { get; set; }
    public double? RemainingAmount { get; set; }
    public double? TotalPriceIncludingVAT { get; set; }
}
