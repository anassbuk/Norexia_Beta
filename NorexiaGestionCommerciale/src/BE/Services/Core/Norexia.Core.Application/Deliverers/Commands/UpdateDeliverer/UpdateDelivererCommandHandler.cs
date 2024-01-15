using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Deliverers.Commands.UpdateDeliverer;
public class UpdateDelivererCommandHandler : IRequestHandler<UpdateDelivererCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateDelivererCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateDelivererCommand request, CancellationToken cancellationToken)
    {
        var deliverer = await _context.Deliverers
                                    .SingleOrDefaultAsync(t => t.Id == request.Id);
        if (deliverer == null)
            throw new NotFoundException($"Deliverer with id ({request.Id}) not found! ");


        deliverer.Reference = request.Reference;
        deliverer.Type = (DelivererType)request.Type!;
        deliverer.FirstName = request.FirstName;
        deliverer.LastName = request.LastName;
        deliverer.Tel = request.Tel;
        deliverer.ServiceProvider = request.ServiceProvider;
        deliverer.Active = request.Active ?? false;

        await _context.SaveChangesAsync(cancellationToken);

        return deliverer.Id;
    }
}
