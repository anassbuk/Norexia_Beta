using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.DeliveryEntities;

namespace Norexia.Core.Application.Deliverers.Queries.GetDeliverers;
public class DelivererDto : IMapFrom<Deliverer>
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public DelivererType? Type { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Tel { get; set; }
    public string? ServiceProvider { get; set; }
    public bool Active { get; set; }
}
