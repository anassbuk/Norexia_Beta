using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.DeliveryEntities;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Deliverers.Commands.CreateDeliverer;
public class CreateDelivererCommandHandler : IRequestHandler<CreateDelivererCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateDelivererCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateDelivererCommand request, CancellationToken cancellationToken)
    {
        if (_context.Deliverers.Any(p => p.Reference == request.Reference))
            throw new NotFoundException($"Deliverer with reference ({request.Reference}) elready exists!");


        var deliverer = new Deliverer()
        {
            Reference = request.Reference,
            Type = (DelivererType)request.Type!,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Tel = request.Tel,
            ServiceProvider = request.ServiceProvider,
            Active = request.Active ?? false,
        };

        var result = await _context.Deliverers.AddAsync(deliverer, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }
}
