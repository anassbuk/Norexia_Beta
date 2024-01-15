using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.PaymentMeans.Queries.GetPaymentMeans;
public class PaymentMeanDto : IMapFrom<PaymentMean>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
