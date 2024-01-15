using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.StockEntry;
public class StockEntryCommandValidator : AbstractValidator<StockEntryCommand>
{
    public StockEntryCommandValidator()
    {
        RuleFor(t => t.Reference)
            .NotEmpty()
            .WithName("Référence");

        RuleFor(t => t.ProviderId)
            .NotEmpty()
            .WithName("Fournisseur");

        When(t => t.StockEntryOrigin == StockEntryOrigin.PurchaseOrder, () =>
        {
            RuleFor(t => t.PurchaseOrderId)
            .NotEmpty()
            .WithName("Commande d'achat");
        });

        RuleFor(t => t.EntryDate)
            .NotEmpty()
            .WithName("Date");

        RuleFor(t => t.StockEntryLines)
            .NotEmpty()
            .WithName("Lignes de commande");

        When(t => t.StockEntryLines != null, () =>
        {
            RuleFor(t => t.StockEntryLines)
            .Must(l => l!.Count > 0)
                .WithName("Lignes de commande");
        });
    }
}
