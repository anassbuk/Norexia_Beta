using FluentValidation;

namespace Norexia.Core.Application.VATs.Commands.CreateVAT;

public class CreateVATCommandValidator : AbstractValidator<CreateVATCommand>
{
    public CreateVATCommandValidator()
    {
        RuleFor(t => t.Value)
            .NotNull()
            .InclusiveBetween(0, 1);
    }
}
