using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Provider;
public class ProviderAddressDtoValidator : AbstractValidator<ProviderAddressDto>
{
    public ProviderAddressDtoValidator()
    {
        RuleFor(t => t.StreetAdress)
            .NotEmpty()
            .WithName("Adresse");

        RuleFor(t => t.City)
            .NotEmpty()
            .WithName("Ville");
    }
}
