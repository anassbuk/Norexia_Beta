using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.PaymentEntities;

namespace Norexia.Core.Application.ProviderPayments.Commands.CreateProviderPayment;
public class CreateProviderPaymentCommandHandler : IRequestHandler<CreateProviderPaymentCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateProviderPaymentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProviderPaymentCommand request, CancellationToken cancellationToken)
    {
        if (await _context.ProviderInvoicePayments.AnyAsync(o => o.Reference == request.Reference))
            throw new NotFoundException($"Payment with reference ({request.Reference}) already exist! ");

        if (!await _context.ProviderInvoices.AnyAsync(o => o.Id == request.ProviderInvoiceId))
            throw new NotFoundException($"Provider Invoice with id ({request.ProviderInvoiceId}) not found! ");

        if (!await _context.PaymentMeans.AnyAsync(c => c.Id == request.PaymentMeanId, cancellationToken))
            throw new NotFoundException($"Payment mean with id ({request.PaymentMeanId}) not found! ");

        ProviderInvoicePayment payment = new()
        {
            Id = Guid.NewGuid(),
            Reference = request.Reference,
            ProviderInvoiceId = request.ProviderInvoiceId,
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

        var result = await _context.ProviderInvoicePayments.AddAsync(payment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}
