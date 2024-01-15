using FluentValidation;
using NorexiaGestionCommercialeWebUI.Models.Purchase;

namespace NorexiaGestionCommercialeWebUI.Models;
public class PurchaseCommandValidator : AbstractValidator<PurchaseCommand>
{
    public PurchaseCommandValidator()
    {
        RuleFor(t => t.ProviderId)
            .NotEmpty()
            .WithName("Fournisseur");

        RuleFor(t => t.OrderDate)
            .NotEmpty()
            .WithName("Date");

        RuleFor(t => t.Reference)
            .NotEmpty()
            .WithName("Référence");

        RuleFor(t => t.PurchaseOrderLines)
            .NotEmpty()
            .WithName("Lignes de commande");

        When(t => t.PurchaseOrderLines != null, () =>
        {
            RuleFor(t => t.PurchaseOrderLines)
            .Must(l => l!.Count > 0)
                .WithName("Lignes de commande");
        });
    }
}
