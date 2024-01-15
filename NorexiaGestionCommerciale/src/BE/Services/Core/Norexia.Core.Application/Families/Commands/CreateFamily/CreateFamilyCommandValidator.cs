using FluentValidation;

namespace Norexia.Core.Application.Families.Commands.CreateFamily;

public class CreateFamilyCommandValidator : AbstractValidator<CreateFamilyCommand>
{
    public CreateFamilyCommandValidator()
    {
        RuleFor(t => t.Designation)
            .MaximumLength(100)
            .NotEmpty();
    }
}
