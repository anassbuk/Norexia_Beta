using FluentValidation;
using static NorexiaGestionCommercialeWebUI.Components.Product.ProductSalesComponent;

namespace NorexiaGestionCommercialeWebUI.Models.Product;
public class SellingPriceValidator : AbstractValidator<SellingPrice>
{
    public SellingPriceValidator()
    {
        RuleFor(t => t.PurchasePrice)
            .NotEmpty().WithMessage("Le prix d'achat est requise");

        RuleFor(t => t.PriceGroupId)
            .NotEmpty().WithMessage("Le groupe de prix est requise");

        RuleFor(t => t.Price)
            .NotEmpty().WithMessage("Le prix de vente est requise");

        RuleFor(t => t.Margin)
            .NotEmpty().WithMessage("La marge est requise");

        RuleFor(t => t.Vat)
            .NotEmpty().WithMessage("TVA est requise");
    }
}
