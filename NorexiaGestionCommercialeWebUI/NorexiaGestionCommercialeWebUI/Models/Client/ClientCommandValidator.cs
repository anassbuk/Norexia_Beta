using FluentValidation;

using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Client;
public class ClientCommandValidator : AbstractValidator<ClientCommand>
{
    public ClientCommandValidator()
    {
        RuleFor(t => t.Reference)
            .NotEmpty()
            .WithName("Référence");

        RuleFor(t => t.ClientType)
                .NotEmpty()
            .WithName("Type de client");

        RuleFor(t => t.LastName)
           .MaximumLength(100)
           .NotEmpty()
            .WithName("Nom");

        RuleFor(t => t.FirstName)
           .MaximumLength(100)
           .NotEmpty()
            .WithName("Prénom");

        When(t => t.ClientType == ClientType.Company, () =>
        {
            RuleFor(t => t.SocialReason)
           .MaximumLength(300)
           .NotEmpty()
            .WithName("Raison social");

            RuleFor(t => t.ICE)
              .MaximumLength(100)
           .NotEmpty()
            .WithName("ICE");

            RuleFor(t => t.CompanyEmail)
         .EmailAddress()
          .WithName("Email");
        });

        RuleFor(t => t.Tel)
          .MaximumLength(20)
           .NotEmpty()
           .WithName("Téléphone");

        RuleFor(t => t.Email)
          .EmailAddress()
           .WithName("Email");


    }
}
