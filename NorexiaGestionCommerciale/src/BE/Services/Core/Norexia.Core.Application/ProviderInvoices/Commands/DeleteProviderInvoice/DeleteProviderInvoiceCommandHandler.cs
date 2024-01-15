using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.ProviderInvoices.Commands.DeleteProviderInvoice;
public record DeleteProviderInvoiceCommand(IEnumerable<Guid> Ids) : IRequest;
public class DeleteProviderInvoiceCommandHandler : IRequestHandler<DeleteProviderInvoiceCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProviderInvoiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProviderInvoiceCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var Invoice = await _context.ProviderInvoices.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (Invoice == null)
            {
                throw new NotFoundException($"Provider invoice with id ({id}) not found!");
            }

            Invoice.IsDeleted = true;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
