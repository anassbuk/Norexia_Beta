using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Payment;
public class PaymentCommandValidator : AbstractValidator<PaymentCommand>
{
    public PaymentCommandValidator()
    {
        RuleFor(t => t.Reference)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.EntryDate)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.PaymentOrigin)
            .NotNull();

        RuleFor(t => t.DueDate)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.PaymentMeanId)
            .NotNull()
            .NotEmpty();


        When(t => t.PaymentOrigin == PaymentOrigin.Invoice, () =>
        {
            RuleFor(t => t.InvoiceId)
                .NotNull()
                .NotEmpty()
                .WithName("Facture");
        });

        When(t => t.PaymentOrigin == PaymentOrigin.SaleOrder, () =>
        {
            RuleFor(t => t.SaleOrderId)
                .NotNull()
                .NotEmpty()
                .WithName("Commande de vente");
        });
    }
}
