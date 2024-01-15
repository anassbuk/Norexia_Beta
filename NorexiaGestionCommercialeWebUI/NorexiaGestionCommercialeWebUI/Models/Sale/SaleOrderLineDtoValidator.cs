using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Sale;
public class SaleOrderLineDtoValidator : AbstractValidator<SaleOrderLineDto>
{
    public SaleOrderLineDtoValidator()
    {
        RuleFor(t => t.Qty)
            .NotEmpty()
            .Must(t => t!.Value > 0)
            .WithName("Qte");

        RuleFor(t => t.SellingPriceId)
            .NotEmpty()
            .WithName("Prix de vente");
    }
}
