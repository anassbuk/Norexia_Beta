using FluentValidation;

namespace NorexiaGestionCommercialeWebUI.Models.Product;
public class ProductCommandValidator : AbstractValidator<ProductCommand>
{
    public ProductCommandValidator()
    {
        RuleFor(t => t.ShortDesignation)
            .NotEmpty().WithMessage("La désignation du produit est requise")
                .MaximumLength(100).WithMessage("La désignation du produit ne doit pas dépasser 100 caractères");

        RuleFor(t => t.Action).NotNull().WithMessage("L'action du produit est requise");

        RuleFor(t => t.Reference).NotEmpty().WithMessage("Le référence du produit est requise");

        RuleFor(t => t.Description).NotEmpty().WithMessage("La description du produit est requise");

        RuleFor(t => t.LongDesignation)
            .NotEmpty().WithMessage("La désignation longue du produit est requise")
                .MaximumLength(1000).WithMessage("La désignation longue du produit ne doit pas dépasser 1000 caractères");

        RuleFor(t => t.Type)
            .NotNull().WithMessage("Le type du produit est requise");

        //RuleFor(t => t.ProductUnits)
        //    .NotEmpty().WithMessage("L'unité par défaut du produit est requise");

        RuleFor(t => t.ProductAvailabilities)
            .NotEmpty().WithMessage("Le canau de vente et requis");

        When(t => t.StorageSupplyInfo != null,
            () =>
            RuleFor
            (c => c.StorageSupplyInfo!.Quantity)
            .NotNull().WithMessage("La quantité est requise")
            .GreaterThanOrEqualTo(0).WithMessage("La quantité est requise"));
    }
}
