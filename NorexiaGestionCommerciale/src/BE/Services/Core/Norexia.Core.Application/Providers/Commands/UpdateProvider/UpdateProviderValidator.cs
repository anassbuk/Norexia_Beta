using FluentValidation;

using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.Providers.Commands.UpdateProvider;
public class UpdateProviderValidator : AbstractValidator<UpdateProvider>
{
    public UpdateProviderValidator()
    {
        RuleFor(t => t.Id)
            .NotEmpty();

        RuleFor(t => t.Reference)
       .NotEmpty();

        RuleFor(t => t.ProviderType)
            .NotEmpty();

        RuleFor(t => t.LastName)
           .MaximumLength(100)
           .NotEmpty();

        RuleFor(t => t.FirstName)
           .MaximumLength(100)
           .NotEmpty();

        When(t => t.ProviderType == ProviderType.Company, () =>
        {
            RuleFor(t => t.SocialReason)
           .MaximumLength(300)
           .NotEmpty();

            RuleFor(t => t.ICE)
            .MaximumLength(100)
            .NotEmpty();
        });

        RuleFor(t => t.Tel)
          .MaximumLength(20)
            .NotEmpty();
    }
}
