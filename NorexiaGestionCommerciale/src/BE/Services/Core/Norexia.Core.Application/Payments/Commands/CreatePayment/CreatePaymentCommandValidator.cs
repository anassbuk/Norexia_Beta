using FluentValidation;

using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.Payments.Commands.CreatePayment;
public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(t => t.Reference)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.EntryDate)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.PaymentOrigin)
            .NotNull();

        RuleFor(t => t.DueDate)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.PaymentMeanId)
            .NotNull()
            .NotEmpty();


        When(t => t.PaymentOrigin == PaymentOrigin.Invoice, () =>
        {
            RuleFor(t => t.InvoiceId)
                .NotNull()
                .NotEmpty();
        });

        When(t => t.PaymentOrigin == PaymentOrigin.SaleOrder, () =>
        {
            RuleFor(t => t.SaleOrderId)
                .NotNull()
                .NotEmpty();
        });
    }
}
