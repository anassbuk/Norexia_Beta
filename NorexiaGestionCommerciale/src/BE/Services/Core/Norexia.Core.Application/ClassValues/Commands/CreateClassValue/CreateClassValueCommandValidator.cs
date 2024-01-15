using FluentValidation;

namespace Norexia.Core.Application.ClassValues.Commands.CreateClassValue;

public class CreateClassValueCommandValidator : AbstractValidator<CreateClassValueCommand>
{
    public CreateClassValueCommandValidator()
    {
        RuleFor(t => t.Value)
            .MaximumLength(100)
            .NotEmpty();
    }
}
