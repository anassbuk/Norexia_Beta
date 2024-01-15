using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Payments.Commands.DeletePayment;
using Norexia.Core.Domain.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.ProviderPayments.Commands.DeleteProviderPayment;
public record DeleteProviderPaymentCommand(IEnumerable<Guid> Ids) : IRequest;
public class DeleteProviderPaymentCommandHandler : IRequestHandler<DeleteProviderPaymentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProviderPaymentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProviderPaymentCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var payment = await _context.ProviderInvoicePayments.FindAsync(id, cancellationToken)
                                            ?? throw new NotFoundException($"Provider Payment with id ({id}) not found!");
            payment!.IsDeleted = true;

        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
