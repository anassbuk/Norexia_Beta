using FluentValidation;

namespace Norexia.Core.Application.ClassValues.Commands.UpdateClassValue;

public class UpdateClassValueCommandValidator : AbstractValidator<UpdateClassValueCommand>
{
    public UpdateClassValueCommandValidator()
    {
        RuleFor(t => t.Value)
            .MaximumLength(100)
            .NotEmpty();
    }
}
