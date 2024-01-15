using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Provider;

namespace NorexiaGestionCommercialeWebUI.Models.Deliverer;
public class DelivererDtoValidator : AbstractValidator<DelivererDto>
{
    public DelivererDtoValidator()
    {
        RuleFor(t => t.Reference)
            .NotEmpty()
            .WithName("Référence");

        RuleFor(t => t.Type)
                .NotEmpty()
            .WithName("Type de livreur");

        RuleFor(t => t.LastName)
           .MaximumLength(100)
           .NotEmpty()
            .WithName("Nom");

        RuleFor(t => t.FirstName)
           .MaximumLength(100)
           .NotEmpty()
            .WithName("Prénom");

        RuleFor(t => t.Tel)
          .MaximumLength(20)
           .NotEmpty()
           .WithName("Téléphone");
    }
}
