using FluentValidation;
using NorexiaGestionCommercialeWebUI.Components.Product;

namespace NorexiaGestionCommercialeWebUI.Models.Product
{
    public class ProductNoteValidator : AbstractValidator<ProductNote>
    {
        public ProductNoteValidator()
        {
            RuleFor(n => n.Note)
                .NotEmpty()
                .WithMessage("La note est requise");
        }
    }
}
