using FluentValidation;

namespace Norexia.Core.Application.ProductAvailabilities.Commands.CreateProductAvailability;
public class CreateProductAvailabilityCommandValidator : AbstractValidator<CreateProductAvailabilityCommand>
{
    public CreateProductAvailabilityCommandValidator()
    {
        RuleFor(t => t.Name)
            .MaximumLength(100)
            .NotEmpty();
    }
}
