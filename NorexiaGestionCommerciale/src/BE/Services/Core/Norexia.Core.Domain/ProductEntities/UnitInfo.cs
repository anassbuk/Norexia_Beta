using Microsoft.EntityFrameworkCore;

namespace Norexia.Core.Domain.ProductEntities;
[Owned]
public class UnitInfo
{
    public bool IsBalance { get; set; } = false;
    public bool IsDecimal { get; set; } = false;
}
