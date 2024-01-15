using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Product;
public class PriceGroupDtoValidator : AbstractValidator<PriceGroupDto>
{
    public PriceGroupDtoValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty()
            .WithMessage("La désignation du groupe de prix est requise");
    }
}
