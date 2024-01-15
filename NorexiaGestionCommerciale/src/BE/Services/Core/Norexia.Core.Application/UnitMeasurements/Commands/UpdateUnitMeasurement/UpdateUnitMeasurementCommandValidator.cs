using FluentValidation;

namespace Norexia.Core.Application.UnitMeasurements.Commands.UpdateUnitMeasurement;

public class UpdateUnitMeasurementCommandValidator : AbstractValidator<UpdateUnitMeasurementCommand>
{
    public UpdateUnitMeasurementCommandValidator()
    {
        RuleFor(t => t.Name)
            .MaximumLength(100)
            .NotEmpty();
    }
}
