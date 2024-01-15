using FluentValidation;
using Norexia.Core.Application.ProviderPayments.Commands.CreateProviderPayment;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.ProviderPayments.Commands.UpdateProviderPayment;
public class UpdateProviderPaymentCommandValidator : AbstractValidator<UpdateProviderPaymentCommand>
{
    public UpdateProviderPaymentCommandValidator()
    {
        RuleFor(t => t.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.Reference)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.EntryDate)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.DueDate)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.PaymentMeanId)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.ProviderInvoiceId)
            .NotNull()
            .NotEmpty();
    }
}
