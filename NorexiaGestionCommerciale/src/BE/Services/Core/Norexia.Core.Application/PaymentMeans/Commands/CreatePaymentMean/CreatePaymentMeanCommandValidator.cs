using FluentValidation;

namespace Norexia.Core.Application.PaymentMeans.Commands.CreatePaymentMean;

internal class CreatePaymentMeanCommandValidator : AbstractValidator<CreatePaymentMeanCommand>
{
    public CreatePaymentMeanCommandValidator()
    {
        RuleFor(t => t.Name)
            .MaximumLength(100)
            .NotEmpty();
    }
}
