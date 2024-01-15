using Microsoft.EntityFrameworkCore;

namespace Norexia.Core.Domain.ProductEntities;
[Owned]
public class PurchaseInfo
{
    public decimal? Price { get; set; }
}
