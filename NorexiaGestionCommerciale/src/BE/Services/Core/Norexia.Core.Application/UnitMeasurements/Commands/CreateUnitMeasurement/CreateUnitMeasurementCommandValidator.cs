using FluentValidation;

namespace Norexia.Core.Application.UnitMeasurements.Commands.CreateUnitMeasurement;

public class CreateUnitMeasurementCommandValidator : AbstractValidator<CreateUnitMeasurementCommand>
{
    public CreateUnitMeasurementCommandValidator()
    {
        RuleFor(t => t.Name)
            .MaximumLength(100)
            .NotEmpty();
    }
}
