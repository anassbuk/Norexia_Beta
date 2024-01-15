using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Payments.Commands.DeletePayment;
public record DeletePaymentCommand(IEnumerable<Guid> Ids) : IRequest;
public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePaymentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            if (await _context.InvoicePayments.AnyAsync(t => t.Id == id, cancellationToken))
            {
                var payment = await _context.InvoicePayments.FindAsync(id, cancellationToken);
                payment!.IsDeleted = true;
            }
            else if (await _context.SalePayments.AnyAsync(t => t.Id == id, cancellationToken))
            {
                var payment = await _context.SalePayments.FindAsync(id, cancellationToken);
                payment!.IsDeleted = true;
            }
            else
                throw new NotFoundException($"Payment with id ({id}) not found!");
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
