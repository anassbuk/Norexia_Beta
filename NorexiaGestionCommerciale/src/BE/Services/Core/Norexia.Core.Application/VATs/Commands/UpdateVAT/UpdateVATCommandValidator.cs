using FluentValidation;

namespace Norexia.Core.Application.VATs.Commands.UpdateVAT;

public class UpdateVATCommandValidator : AbstractValidator<UpdateVATCommand>
{
    public UpdateVATCommandValidator()
    {
        RuleFor(t => t.Id)
            .NotEmpty();

        RuleFor(t => t.Value)
            .NotNull()
            .InclusiveBetween(0, 1);
    }
}
