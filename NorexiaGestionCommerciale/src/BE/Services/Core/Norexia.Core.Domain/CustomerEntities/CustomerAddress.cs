using Norexia.Core.Domain.Common;

namespace Norexia.Core.Domain.CustomerEntities;

public class CustomerAddress : AddressBase
{
    public Guid CustomerId { get; set; }
    public virtual Customer? Customer { get; set; }
}
