using FluentValidation;

namespace Norexia.Core.Application.PaymentMeans.Commands.UpdatePaymentMean;

public class UpdatePaymentMeanCommandValidator : AbstractValidator<UpdatePaymentMeanCommand>
{
    public UpdatePaymentMeanCommandValidator()
    {
        RuleFor(t => t.Name)
            .MaximumLength(100)
            .NotEmpty();
    }
}
