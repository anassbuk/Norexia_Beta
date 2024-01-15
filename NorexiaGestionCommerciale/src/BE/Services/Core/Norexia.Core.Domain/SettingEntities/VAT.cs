using Norexia.Core.Domain.Common;

namespace Norexia.Core.Domain.SettingEntities;

public class VAT : BaseAuditableEntity
{
    public decimal Value { get; set; }
    public bool IsDefault { get; set; }
}
