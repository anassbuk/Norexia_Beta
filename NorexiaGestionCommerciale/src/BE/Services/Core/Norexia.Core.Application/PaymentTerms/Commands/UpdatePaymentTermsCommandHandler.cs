using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.PaymentTerms.Commands;

public class UpdatePaymentTermsCommandHandler : IRequestHandler<UpdatePaymentTermsCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdatePaymentTermsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdatePaymentTermsCommand request, CancellationToken cancellationToken)
    {
        if (_context.PaymentTerms.Any())
        {
            var terms = await _context.PaymentTerms.FirstAsync();
            terms.MaturityDuration = request.MaturityDuration;
            terms.MaturityDurationNegotiable = request.MaturityDurationNegotiable;
            terms.DepositInvoice = request.DepositInvoice;
            terms.DepositInvoiceNegotiable = request.DepositInvoiceNegotiable;
            terms.DepositInvoiceDownPayment = request.DepositInvoiceDownPayment;
            terms.PaymentByInstallments = request.PaymentByInstallments;
            terms.PaymentByInstallmentsNegotiable = request.PaymentByInstallmentsNegotiable;
            terms.PaymentByInstallmentsNumber = request.PaymentByInstallmentsNumber;
            if (request.PaymentOption != null)
                terms.PaymentOption = (PaymentOption)request.PaymentOption;
        }
        else
        {
            var terms = new Domain.SaleOrderEntities.PaymentTerms
            {
                MaturityDuration = request.MaturityDuration,
                MaturityDurationNegotiable = request.MaturityDurationNegotiable,
                DepositInvoice = request.DepositInvoice,
                DepositInvoiceNegotiable = request.DepositInvoiceNegotiable,
                DepositInvoiceDownPayment = request.DepositInvoiceDownPayment,
                PaymentByInstallments = request.PaymentByInstallments,
                PaymentByInstallmentsNegotiable = request.PaymentByInstallmentsNegotiable,
                PaymentByInstallmentsNumber = request.PaymentByInstallmentsNumber,
            };

            if (request.PaymentOption != null)
                terms.PaymentOption = (PaymentOption)request.PaymentOption;

            var result = await _context.PaymentTerms.AddAsync(terms, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
