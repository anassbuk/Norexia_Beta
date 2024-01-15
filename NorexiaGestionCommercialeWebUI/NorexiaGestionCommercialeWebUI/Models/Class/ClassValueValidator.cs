using FluentValidation;
using static NorexiaGestionCommercialeWebUI.Components.Product.ProductClassificationComponent;

namespace NorexiaGestionCommercialeWebUI.Models.Class;
public class ClassValueValidator : AbstractValidator<ClassValue>
{
    public ClassValueValidator()
    {
        RuleFor(c => c.ClassValueId).NotEmpty().WithMessage("La classe est requise");
        RuleFor(c => c.ClassId).NotEmpty().WithMessage("La valeur est requise");
    }
}
