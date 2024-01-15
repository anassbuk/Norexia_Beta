using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.SettingEntities;

namespace Norexia.Core.Application.VATs.Queries.GetVATs;

public class VATDto : IMapFrom<VAT>
{
    public Guid? Id { get; set; }
    public decimal? Value { get; set; }
    public bool IsDefault { get; set; }
}
