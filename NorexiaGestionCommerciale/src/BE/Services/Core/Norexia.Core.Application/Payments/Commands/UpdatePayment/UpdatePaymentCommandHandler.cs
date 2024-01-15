using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Payments.Commands.UpdatePayment;
public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public UpdatePaymentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
    {
        if (request.PaymentOrigin == Domain.Common.Enums.PaymentOrigin.Invoice)
        {
            var payment = await _context.InvoicePayments
                                    .FindAsync(request.Id, cancellationToken)
                                        ?? throw new NotFoundException($"Payment with id ({request.Id}) not found!");

            if (!await _context.Invoices.AnyAsync(o => o.Id == request.InvoiceId))
                throw new NotFoundException($"Invoice with id ({request.InvoiceId}) not found! ");

            if (!await _context.PaymentMeans.AnyAsync(c => c.Id == request.PaymentMeanId, cancellationToken))
                throw new NotFoundException($"Payment mean with id ({request.PaymentMeanId}) not found!");

            payment.InvoiceId = request.InvoiceId;
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
        else
        {
            var payment = await _context.SalePayments
                                    .FindAsync(request.Id, cancellationToken)
                                        ?? throw new NotFoundException($"Payment with id ({request.Id}) not found!");

            if (!await _context.SaleOrders.AnyAsync(o => o.Id == request.SaleOrderId))
                throw new NotFoundException($"Sale order with id ({request.SaleOrderId}) not found! ");

            if (!await _context.PaymentMeans.AnyAsync(c => c.Id == request.PaymentMeanId, cancellationToken))
                throw new NotFoundException($"Payment mean with id ({request.PaymentMeanId}) not found! ");


            payment.SaleOrderId = request.SaleOrderId;
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
}
