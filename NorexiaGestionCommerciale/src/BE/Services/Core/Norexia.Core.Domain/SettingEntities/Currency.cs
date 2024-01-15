using Norexia.Core.Domain.Common;

namespace Norexia.Core.Domain.SettingEntities;

public class Currency : BaseAuditableEntity
{
    public string? Name { get; set; }
    public bool IsDefault { get; set; }
}
