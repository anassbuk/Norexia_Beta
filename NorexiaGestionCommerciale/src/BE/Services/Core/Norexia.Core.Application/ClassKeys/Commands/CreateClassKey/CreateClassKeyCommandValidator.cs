using FluentValidation;

namespace Norexia.Core.Application.ClassKeys.Commands.CreateClassKey;

public class CreateClassKeyCommandValidator : AbstractValidator<CreateClassKeyCommand>
{
    public CreateClassKeyCommandValidator()
    {
        RuleFor(t => t.Key)
            .MaximumLength(100)
            .NotEmpty();
    }
}
