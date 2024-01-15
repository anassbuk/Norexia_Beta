using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Customers.Commands.DeleteCustomer;

public record DeleteCustomer(IEnumerable<Guid> Ids) : IRequest;
internal class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomer>
{
    private readonly IApplicationDbContext _context;

    public DeleteCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCustomer request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(t => t.Id == id);
            if (customer == null)
            {
                throw new NotFoundException($"Customer with id {id} not found! ");
            }

            customer.IsDeleted = true;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
