using Norexia.Core.Domain.Common;

namespace Norexia.Core.Domain.ProviderEntities;
public class ProviderAddress : AddressBase
{
    public Guid ProviderId { get; set; }
    public virtual Provider? Provider { get; set; }
}

