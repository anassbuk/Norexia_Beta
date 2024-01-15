using MediatR;

namespace Norexia.Core.Application.ProductAvailabilities.Commands.CreateProductAvailability;
public class CreateProductAvailabilityCommand : IRequest<Guid>
{
    public string? Name { get; set; }
}
