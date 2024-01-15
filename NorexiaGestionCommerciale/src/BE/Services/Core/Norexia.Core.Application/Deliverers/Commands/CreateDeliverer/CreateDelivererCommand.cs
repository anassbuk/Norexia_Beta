using MediatR;

using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.Deliverers.Commands.CreateDeliverer;
public class CreateDelivererCommand : IRequest<Guid>
{
    public string? Reference { get; set; }
    public DelivererType? Type { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Tel { get; set; }
    public string? ServiceProvider { get; set; }
    public bool? Active { get; set; }
}
