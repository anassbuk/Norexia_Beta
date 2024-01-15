using FluentValidation;
using NorexiaGestionCommercialeWebUI.Models.Sale;

namespace NorexiaGestionCommercialeWebUI.Models;
public class SaleCommandValidator : AbstractValidator<SaleCommand>
{
    public SaleCommandValidator()
    {
        RuleFor(t => t.OperationType)
            .NotEmpty()
            .WithName("Type d’opération");

        RuleFor(t => t.Execution)
            .NotEmpty()
            .WithName("Exécution");

        RuleFor(t => t.OrderDate)
            .NotEmpty()
            .WithName("Date");

        RuleFor(t => t.Reference)
            .NotEmpty()
            .WithName("Référence");

        RuleFor(t => t.SaleOrderLines)
            .NotEmpty()
            .WithName("Lignes de commande");

        When(t => t.SaleOrderOrigin == Norexia.Core.Facade.Client.Sdk.SaleOrderOrigin.Quotation, () =>
        {
            RuleFor(t => t.QuotationId)
            .NotEmpty()
                .WithName("Devis");
        });

        When(t => t.SaleOrderLines != null, () =>
        {
            RuleFor(t => t.SaleOrderLines)
            .Must(l => l!.Count > 0)
                .WithName("Lignes de commande");
        });

        When(t => t.PaymentTerms != null && t.PaymentTerms!.PaymentByInstallments == true, () =>
        {
            RuleFor(t => t.SalePayments)
                .NotEmpty()
                .WithMessage("Merci d'ajouter les règlements planifié");


            When(t => t.SalePayments != null, () =>
            {
                RuleFor(t => t.SalePayments)
                .Must((t, l) => l!.Count >= t.PaymentTerms!.PaymentByInstallmentsNumber)
                .WithMessage("Merci d'ajouter les règlements planifié");
            });
        });
    }
}
