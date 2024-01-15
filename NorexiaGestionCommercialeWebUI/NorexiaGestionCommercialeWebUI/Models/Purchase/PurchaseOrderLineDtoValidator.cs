using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Purchase;
public class PurchaseOrderLineDtoValidator : AbstractValidator<PurchaseOrderLineDto>
{
    public PurchaseOrderLineDtoValidator()
    {
        RuleFor(t => t.Qty)
            .NotEmpty()
            .Must(t => t!.Value > 0)
            .WithName("Qte");

        RuleFor(t => t.Price)
            .NotEmpty()
            .WithName("Prix d'achat");
    }
}
