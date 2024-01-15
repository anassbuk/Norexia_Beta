using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Currency
{
    public class CurrencyDtoValidator : AbstractValidator<CurrencyDto>
    {
        public CurrencyDtoValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty()
                .WithName("Devise");
        }
    }
}
