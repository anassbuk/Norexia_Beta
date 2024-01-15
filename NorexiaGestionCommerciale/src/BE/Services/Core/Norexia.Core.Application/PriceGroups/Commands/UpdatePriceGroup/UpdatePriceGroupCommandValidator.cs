using FluentValidation;

namespace Norexia.Core.Application.PriceGroups.Commands.UpdatePriceGroup;

public class UpdatePriceGroupCommandValidator : AbstractValidator<UpdatePriceGroupCommand>
{
    public UpdatePriceGroupCommandValidator()
    {
        RuleFor(t => t.Name)
            .MaximumLength(100)
            .NotEmpty();
    }
}
