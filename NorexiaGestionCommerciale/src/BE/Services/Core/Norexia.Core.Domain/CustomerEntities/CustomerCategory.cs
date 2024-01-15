using Norexia.Core.Domain.Common;

namespace Norexia.Core.Domain.CustomerEntities;
public class CustomerCategory : BaseAuditableEntity
{
    public string? Name { get; set; }
}
