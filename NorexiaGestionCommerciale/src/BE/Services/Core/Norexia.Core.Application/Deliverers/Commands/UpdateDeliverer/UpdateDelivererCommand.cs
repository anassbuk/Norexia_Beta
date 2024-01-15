using MediatR;

using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.Deliverers.Commands.UpdateDeliverer;
public class UpdateDelivererCommand : IRequest<Guid>
{
    public Guid? Id { get; set; }
    public string? Reference { get; set; }
    public DelivererType? Type { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Tel { get; set; }
    public string? ServiceProvider { get; set; }
    public bool? Active { get; set; }
}
