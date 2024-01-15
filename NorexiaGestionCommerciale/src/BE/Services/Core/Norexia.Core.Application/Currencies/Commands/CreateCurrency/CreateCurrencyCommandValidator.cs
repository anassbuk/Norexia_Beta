using FluentValidation;

namespace Norexia.Core.Application.Currencies.Commands.CreateCurrency;

public class CreateCurrencyCommandValidator : AbstractValidator<CreateCurrencyCommand>
{
    public CreateCurrencyCommandValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty();
    }
}
