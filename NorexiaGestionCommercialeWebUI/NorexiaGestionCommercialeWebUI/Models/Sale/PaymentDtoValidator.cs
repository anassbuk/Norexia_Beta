using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Sale
{
    public class PaymentDtoValidator : AbstractValidator<PaymentDto>
    {
        public PaymentDtoValidator()
        {
            RuleFor(t => t.Reference)
                .NotEmpty()
                .WithName("Référence");

            RuleFor(t => t.EntryDate)
                .NotEmpty()
                .WithName("Date création");

            RuleFor(t => t.DueDate)
                .NotEmpty()
                .WithName("Date d'échéance");

            RuleFor(t => t.PaymentMeanId)
                .NotEmpty()
                .WithName("Moyen de paiment");
        }
    }
}
