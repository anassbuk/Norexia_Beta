using FluentValidation;

namespace Norexia.Core.Application.ProviderPayments.Commands.CreateProviderPayment;
public class CreateProviderPaymentCommandValidator : AbstractValidator<CreateProviderPaymentCommand>
{
    public CreateProviderPaymentCommandValidator()
    {
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
