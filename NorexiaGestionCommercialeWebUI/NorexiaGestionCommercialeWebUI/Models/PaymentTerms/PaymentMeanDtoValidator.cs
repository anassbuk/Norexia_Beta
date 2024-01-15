using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.PaymentTerms
{
    public class PaymentMeanDtoValidator : AbstractValidator<PaymentMeanDto>
    {
        public PaymentMeanDtoValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty()
                .WithName("Moyen de paiement");
        }
    }
}
