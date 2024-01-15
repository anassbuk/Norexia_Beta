using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.UnitTypes.Queries.GetUnits;

public class UnitDto : IMapFrom<UnitType>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public ICollection<UnitMeasurementDto>? Measurements { get; set; }
}
