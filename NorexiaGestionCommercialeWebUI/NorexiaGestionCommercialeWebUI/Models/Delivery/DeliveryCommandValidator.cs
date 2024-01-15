using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Delivery;
public class DeliveryCommandValidator : AbstractValidator<DeliveryCommand>
{
    public DeliveryCommandValidator()
    {
        RuleFor(t => t.Reference)
            .NotEmpty()
            .WithName("Référence");

        //RuleFor(t => t.CustomerId)
        //    .NotEmpty()
        //    .WithName("Client");

        RuleFor(t => t.DelivererId)
            .NotEmpty()
            .WithName("Livreur");

        When(t => t.DeliveryOrigin == DeliveryOrigin.SaleOrder, () =>
        {
            RuleFor(t => t.SaleOrderId)
            .NotEmpty()
            .WithName("Commande de vente");
        });

        When(t => t.DeliveryOrigin == DeliveryOrigin.Facture, () =>
        {
            RuleFor(t => t.InvoiceId)
            .NotEmpty()
            .WithName("Facture");
        });

        RuleFor(t => t.EntryDate)
            .NotEmpty()
            .WithName("Date");

        RuleFor(t => t.DeliveryLines)
            .NotEmpty()
            .WithName("Lignes de commande");

        When(t => t.DeliveryLines != null, () =>
        {
            RuleFor(t => t.DeliveryLines)
            .Must(l => l!.Count > 0)
                .WithName("Lignes de commande");
        });
    }
}
