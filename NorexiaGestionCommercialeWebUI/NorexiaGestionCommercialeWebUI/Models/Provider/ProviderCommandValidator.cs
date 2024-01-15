using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Provider;
public class ProviderCommandValidator : AbstractValidator<ProviderCommand>
{
    public ProviderCommandValidator()
    {
        RuleFor(t => t.Reference)
            .NotEmpty()
            .WithName("Référence");

        RuleFor(t => t.ProviderType)
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

        When(t => t.ProviderType == ProviderType.Company, () =>
        {
            RuleFor(t => t.SocialReason)
           .MaximumLength(300)
           .NotEmpty()
            .WithName("Raison social");

            RuleFor(t => t.ICE)
              .MaximumLength(100)
           .NotEmpty()
            .WithName("ICE");

            When(t => !string.IsNullOrEmpty(t.CompanyEmail), () =>
            {
                RuleFor(t => t.CompanyEmail)
              .EmailAddress()
               .WithName("Email");
            });
        });

        RuleFor(t => t.Tel)
          .MaximumLength(20)
           .NotEmpty()
           .WithName("Téléphone");

        When(t => !string.IsNullOrEmpty(t.Email), () =>
        {
            RuleFor(t => t.Email)
          .EmailAddress()
           .WithName("Email");
        });
    }
}
