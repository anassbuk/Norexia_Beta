using FluentValidation;
using NorexiaGestionCommercialeWebUI.Models.Quotation;
namespace NorexiaGestionCommercialeWebUI.Models;

public class QuotationCommandValidator : AbstractValidator<QuotationCommand>
{
    public QuotationCommandValidator()
    {
        // RuleFor(t => t.CustomerId)
        //    .NotEmpty()
        //    .WithName("Client");


        RuleFor(t => t.QuotationDate)
            .NotEmpty()
            .WithName("Date");

        RuleFor(t => t.Reference)
            .NotEmpty()
            .WithName("Référence");

        RuleFor(t=>t.ValidityDuretion)
            .NotEmpty()
            .WithName("Durée de validité");

        RuleFor(t=>t.Responsable)
            .NotEmpty()
            .WithName("Responsable");

        RuleFor(t => t.Version)
            .NotEmpty()
            .WithName("Version");

        RuleFor(t => t.QuotationLines)
            .NotEmpty()
            .WithName("Lignes de commande");

        When(t => t.QuotationLines != null, () =>
        {
            RuleFor(t => t.QuotationLines)
            .Must(l => l!.Count > 0)
                .WithName("Lignes de commande");
        });

        
    }
}
