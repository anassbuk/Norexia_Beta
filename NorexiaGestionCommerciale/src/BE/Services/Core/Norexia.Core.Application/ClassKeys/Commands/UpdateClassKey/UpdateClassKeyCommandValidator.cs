using FluentValidation;

namespace Norexia.Core.Application.ClassKeys.Commands.UpdateClassKey;

internal class UpdateClassKeyCommandValidator : AbstractValidator<UpdateClassKeyCommand>
{
    public UpdateClassKeyCommandValidator()
    {
        RuleFor(t => t.Key)
            .MaximumLength(100)
            .NotEmpty();
    }
}
