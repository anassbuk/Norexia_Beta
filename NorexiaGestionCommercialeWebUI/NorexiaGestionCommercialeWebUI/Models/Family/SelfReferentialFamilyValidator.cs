using FluentValidation;
using static NorexiaGestionCommercialeWebUI.Components.Product.ProductSalesComponent;
using static NorexiaGestionCommercialeWebUI.Components.Settings.FamilySettings;

namespace NorexiaGestionCommercialeWebUI.Models.Family;
public class SelfReferentialFamilyValidator : AbstractValidator<SelfReferentialFamily>
{
    public SelfReferentialFamilyValidator()
    {
        RuleFor(t => t.Designation)
            .NotEmpty().WithMessage("La désignation est requise");
    }
}
