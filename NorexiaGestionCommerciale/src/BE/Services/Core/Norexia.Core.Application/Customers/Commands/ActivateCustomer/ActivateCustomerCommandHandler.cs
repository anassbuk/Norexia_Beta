using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Customers.Commands.ActivateCustomer;
internal class ActivateCustomerCommandHandler : IRequestHandler<ActivateCustomerCommand>
{
    private readonly IApplicationDbContext _context;
    public ActivateCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ActivateCustomerCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (customer == null)
            {
                throw new NotFoundException($"Customer with id ({id}) not found! ");
            }

            customer.Active = !customer.Active;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
public record ActivateCustomerCommand(IEnumerable<Guid> Ids) : IRequest;
