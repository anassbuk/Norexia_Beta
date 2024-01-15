using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Client;
public class CustomerAddressDtoValidator : AbstractValidator<CustomerAddressDto>
{
    public CustomerAddressDtoValidator()
    {
        RuleFor(t => t.StreetAdress)
            .NotEmpty()
            .WithMessage("L'adresse est requise");

        RuleFor(t => t.City)
            .NotEmpty()
            .WithMessage("La ville est requise");
    }
}
