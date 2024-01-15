using FluentValidation;

namespace Norexia.Core.Application.ProductAvailabilities.Commands.UpdateProductAvailability;

public class UpdateProductAvailabilityCommandValidator : AbstractValidator<UpdateProductAvailabilityCommand>
{
    public UpdateProductAvailabilityCommandValidator()
    {
        RuleFor(t => t.Name)
            .MaximumLength(100)
            .NotEmpty();
    }
}
