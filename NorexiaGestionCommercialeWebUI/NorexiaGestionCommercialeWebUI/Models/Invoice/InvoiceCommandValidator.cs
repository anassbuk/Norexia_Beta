using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Invoice
{
    public class InvoiceCommandValidator : AbstractValidator<InvoiceCommand>
    {
        public InvoiceCommandValidator()
        {
            RuleFor(t => t.Reference)
                .NotEmpty()
                .WithName("Référence");

            RuleFor(t => t.CustomerId)
                .NotEmpty()
                .WithName("Client");


            When(t => (t.InvoiceOrigin == InvoiceOrigin.SalesOrder || 
                        t.InvoiceOrigin == InvoiceOrigin.DeliveryMulti), () =>
            {
                RuleFor(t => t.SaleOrderId)
                .NotEmpty()
                .WithName("Commande de vente");
            });

            //When(t => t.InvoiceOrigin == InvoiceOrigin.DeliveryMono, () =>
            //{
            //    RuleFor(t => t.DeliveryId)
            //    .NotEmpty()
            //    .WithName("Bon de livraison");
            //});

            When(t => t.InvoiceOrigin == InvoiceOrigin.DeliveryMulti, () =>
            {
                RuleFor(t => t.DeliveryStartDate)
                .NotEmpty()
                .WithName("Date de début de livraison");

                RuleFor(t => t.DeliveryEndDate)
                .NotEmpty()
                .WithName("Date de fin de livraison");
            });

            When(t => t.PaymentTerms != null && t.PaymentTerms.DepositInvoice == true, () =>
            {
                RuleFor(t => t.PaymentTerms!.DepositInvoiceDownPayment)
                .NotEmpty()
                .WithName("Montant d’acompte");
            });

            RuleFor(t => t.EntryDate)
                .NotEmpty()
                .WithName("Date");

            RuleFor(t => t.InvoiceLines)
                .NotEmpty()
                .WithName("Lignes de facture");

            When(t => t.InvoiceLines != null, () =>
            {
                RuleFor(t => t.InvoiceLines)
                .Must(l => l!.Count > 0)
                    .WithName("Lignes de facture");
            });



            When(t => t.PaymentTerms != null && t.PaymentTerms!.PaymentByInstallments == true, () =>
            {
                RuleFor(t => t.InvoicePayments)
                    .NotEmpty()
                    .WithMessage("Merci d'ajouter les règlements planifié");


                When(t => t.InvoicePayments != null, () =>
                {
                    RuleFor(t => t.InvoicePayments)
                    .Must((t, l) => l!.Count >= t.PaymentTerms!.PaymentByInstallmentsNumber)
                    .WithMessage("Merci d'ajouter les règlements planifié");
                });
            });
        }
    }
}
