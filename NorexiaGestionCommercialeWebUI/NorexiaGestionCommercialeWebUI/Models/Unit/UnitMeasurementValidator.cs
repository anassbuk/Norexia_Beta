using FluentValidation;
using static NorexiaGestionCommercialeWebUI.Components.Product.ProductUnitComponent;

namespace NorexiaGestionCommercialeWebUI.Models.Unit;
public class UnitMeasurementValidator : AbstractValidator<UnitMeasurement>
{
    public UnitMeasurementValidator()
    {
        RuleFor(t => t.UnitId)
            .NotEmpty().WithMessage("l'unité du produit est requise");

        RuleFor(t => t.MeasurementId)
            .NotEmpty().WithMessage("La mesure du produit est requise");
    }
}
