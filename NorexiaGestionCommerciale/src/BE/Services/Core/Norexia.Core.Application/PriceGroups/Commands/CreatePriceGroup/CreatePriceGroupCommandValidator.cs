using FluentValidation;

namespace Norexia.Core.Application.PriceGroups.Commands.CreatePriceGroup;

public class CreatePriceGroupCommandValidator : AbstractValidator<CreatePriceGroupCommand>
{
    public CreatePriceGroupCommandValidator()
    {
        RuleFor(t => t.Name)
            .MaximumLength(100)
            .NotEmpty();
    }
}
