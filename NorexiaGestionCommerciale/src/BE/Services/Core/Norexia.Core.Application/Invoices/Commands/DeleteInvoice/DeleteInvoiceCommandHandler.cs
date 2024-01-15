using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Invoices.Commands.DeleteInvoice;

public record DeleteInvoiceCommand(IEnumerable<Guid> Ids) : IRequest;
public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteInvoiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var Invoice = await _context.Invoices.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (Invoice == null)
            {
                throw new NotFoundException($"Invoice with id ({id}) not found!");
            }

            Invoice.IsDeleted = true;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
