using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.ProductAvailabilities.Commands.CreateProductAvailability;
public class CreateProductAvailabilityCommandHandler : IRequestHandler<CreateProductAvailabilityCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateProductAvailabilityCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProductAvailabilityCommand request, CancellationToken cancellationToken)
    {
        ProductAvailability availability = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        var result = await _context.ProductAvailabilities.AddAsync(availability, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}
