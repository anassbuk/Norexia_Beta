using FluentValidation;

namespace Norexia.Core.Application.Families.Commands.UpdateFamily;

public class UpdateFamilyCommandValidator : AbstractValidator<UpdateFamilyCommand>
{
    public UpdateFamilyCommandValidator()
    {
        RuleFor(t => t.Designation)
            .MaximumLength(100)
            .NotEmpty();
    }
}
