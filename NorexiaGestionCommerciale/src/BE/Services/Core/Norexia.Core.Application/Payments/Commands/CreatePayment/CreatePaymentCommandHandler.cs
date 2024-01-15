using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.PaymentEntities;

namespace Norexia.Core.Application.Payments.Commands.CreatePayment;
public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreatePaymentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        if(request.PaymentOrigin == PaymentOrigin.Invoice)
        {
            if (await _context.InvoicePayments.AnyAsync(o => o.Reference == request.Reference))
                throw new NotFoundException($"Payment with reference ({request.Reference}) already exist! ");

            if (!await _context.Invoices.AnyAsync(o => o.Id == request.InvoiceId))
                throw new NotFoundException($"Invoice with id ({request.InvoiceId}) not found! ");

            if (!await _context.PaymentMeans.AnyAsync(c => c.Id == request.PaymentMeanId, cancellationToken))
                throw new NotFoundException($"Payment mean with id ({request.PaymentMeanId}) not found! ");

            InvoicePayment payment = new()
            {
                Id = Guid.NewGuid(),
                Reference = request.Reference,
                InvoiceId = request.InvoiceId,
                PaymentMeanId = request.PaymentMeanId,
                EntryDate = request.EntryDate.ToUniversalTime(),
                DueDate = request.DueDate?.ToUniversalTime(),
                OperationDate = request.OperationDate?.ToUniversalTime(),
                OperationNumber = request.OperationNumber,
                Bank = request.Bank,
                Status = request.Status,
                Note = request.Note,
                AmountToBePaid = request.AmountToBePaid,
                AmountToBePaidPercentage = request.AmountToBePaidPercentage,
                AmountPaid = request.AmountPaid,
            };

            var result = await _context.InvoicePayments.AddAsync(payment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return result.Entity.Id;
        }
        else
        {
            if (await _context.SalePayments.AnyAsync(o => o.Reference == request.Reference))
                throw new NotFoundException($"Payment with reference ({request.Reference}) already exist! ");

            if (!await _context.SaleOrders.AnyAsync(o => o.Id == request.SaleOrderId))
                throw new NotFoundException($"Sale order with id ({request.SaleOrderId}) not found! ");

            if (!await _context.PaymentMeans.AnyAsync(c => c.Id == request.PaymentMeanId, cancellationToken))
                throw new NotFoundException($"Payment mean with id ({request.PaymentMeanId}) not found! ");

            SalePayment payment = new()
            {
                Id = Guid.NewGuid(),
                Reference = request.Reference,
                SaleOrderId = request.SaleOrderId,
                PaymentMeanId = request.PaymentMeanId,
                EntryDate = request.EntryDate.ToUniversalTime(),
                DueDate = request.DueDate?.ToUniversalTime(),
                OperationDate = request.OperationDate?.ToUniversalTime(),
                OperationNumber = request.OperationNumber,
                Bank = request.Bank,
                Status = request.Status,
                Note = request.Note,
                AmountToBePaid = request.AmountToBePaid,
                AmountToBePaidPercentage = request.AmountToBePaidPercentage,
                AmountPaid = request.AmountPaid,
            };

            var result = await _context.SalePayments.AddAsync(payment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return result.Entity.Id;
        }
    }
}
