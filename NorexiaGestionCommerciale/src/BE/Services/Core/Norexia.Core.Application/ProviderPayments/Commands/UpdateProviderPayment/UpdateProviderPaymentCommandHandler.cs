using MediatR;
using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.ProviderPayments.Commands.UpdateProviderPayment;
public class UpdateProviderPaymentCommandHandler : IRequestHandler<UpdateProviderPaymentCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public UpdateProviderPaymentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateProviderPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _context.ProviderInvoicePayments
                                    .FindAsync(request.Id, cancellationToken)
                                        ?? throw new NotFoundException($"Provider Payment with id ({request.Id}) not found!");

        if (!await _context.ProviderInvoices.AnyAsync(o => o.Id == request.ProviderInvoiceId))
            throw new NotFoundException($"Invoice with id ({request.ProviderInvoiceId}) not found! ");

        if (!await _context.PaymentMeans.AnyAsync(c => c.Id == request.PaymentMeanId, cancellationToken))
            throw new NotFoundException($"Payment mean with id ({request.PaymentMeanId}) not found!");

        payment.ProviderInvoiceId = request.ProviderInvoiceId;
        payment.PaymentMeanId = request.PaymentMeanId;
        payment.EntryDate = request.EntryDate.ToUniversalTime();
        payment.DueDate = request.DueDate?.ToUniversalTime();
        payment.OperationDate = request.OperationDate?.ToUniversalTime();
        payment.OperationNumber = request.OperationNumber;
        payment.Bank = request.Bank;
        payment.Status = request.Status;
        payment.Note = request.Note;
        payment.AmountToBePaid = request.AmountToBePaid;
        payment.AmountToBePaidPercentage = request.AmountToBePaidPercentage;
        payment.AmountPaid = request.AmountPaid;

        await _context.SaveChangesAsync(cancellationToken);

        return payment.Id;
    }
}
