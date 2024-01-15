using FluentValidation;

namespace Norexia.Core.Application.UnitTypes.Commands.UpdateUnitType;

internal class UpdateUnitTypeCommandValidator : AbstractValidator<UpdateUnitTypeCommand>
{
    public UpdateUnitTypeCommandValidator()
    {
        RuleFor(t => t.Name)
            .MaximumLength(100)
            .NotEmpty();
    }
}
