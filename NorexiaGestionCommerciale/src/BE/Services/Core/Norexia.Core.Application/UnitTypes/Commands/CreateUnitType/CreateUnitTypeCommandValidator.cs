using FluentValidation;

namespace Norexia.Core.Application.UnitTypes.Commands.CreateUnitType;

public class CreateUnitTypeCommandValidator : AbstractValidator<CreateUnitTypeCommand>
{
    public CreateUnitTypeCommandValidator()
    {
        RuleFor(t => t.Name)
            .MaximumLength(100)
            .NotEmpty();
    }
}
