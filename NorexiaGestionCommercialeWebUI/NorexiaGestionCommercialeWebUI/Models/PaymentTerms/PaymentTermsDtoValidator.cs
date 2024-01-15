using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.PaymentTerms
{
    public class PaymentTermsDtoValidator : AbstractValidator<PaymentTermsDto>
    {
        public PaymentTermsDtoValidator()
        {
            RuleFor(t => t.MaturityDuration)
                .GreaterThanOrEqualTo(0)
                .WithName("Durée d’échéance");

            RuleFor(t => t.DepositInvoiceDownPayment)
                .GreaterThanOrEqualTo(0)
                .WithName("Montant d’acompte");

            RuleFor(t => t.PaymentByInstallmentsNumber)
                .GreaterThanOrEqualTo(0)
                .WithName("Paiement échelonné (Nombre de fois)");
        }
    }
}
