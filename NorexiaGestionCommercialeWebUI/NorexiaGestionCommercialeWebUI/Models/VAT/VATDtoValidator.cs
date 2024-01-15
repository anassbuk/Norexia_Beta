using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;
using static NorexiaGestionCommercialeWebUI.Components.Product.ProductUnitComponent;

namespace NorexiaGestionCommercialeWebUI.Models.VAT
{
    public class VATDtoValidator : AbstractValidator<VATDto>
    {
        public VATDtoValidator()
        {
            RuleFor(t => t.Value)
                .NotEmpty()
                .WithName("TVA");
        }
    }
}
