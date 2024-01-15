using FluentValidation;

using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.Deliveries.Commands.CreateDelivery;
public class CreateDeliveryCommandValidator : AbstractValidator<CreateDeliveryCommand>
{
    public CreateDeliveryCommandValidator()
    {
        //RuleFor(t => t.CustomerId)
        //    .NotNull()
        //    .NotEmpty();

        RuleFor(t => t.DelivererId)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.Reference)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.EntryDate)
            .NotNull()
            .NotEmpty();

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

        RuleFor(t => t.DeliveryLines)
            .NotNull()
            .Must(l => l!.Count > 0)
            .WithMessage("The number of lines is insufficient");
    }
}
