using MediatR;

namespace Norexia.Core.Application.ProductAvailabilities.Commands.UpdateProductAvailability;

public class UpdateProductAvailabilityCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
