using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Product
{
    public class ProductAvailabilityDtoValidator : AbstractValidator<ProductAvailabilityDto>
    {
        public ProductAvailabilityDtoValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty()
                .WithMessage("La désignation du canal de vente est requise");
        }
    }
}
