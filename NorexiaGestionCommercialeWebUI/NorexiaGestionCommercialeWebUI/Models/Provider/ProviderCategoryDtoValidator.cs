using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Provider;
public class ProviderCategoryDtoValidator : AbstractValidator<ProviderCategoryDto>
{
    public ProviderCategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .NotEmpty()
            .WithName("Catégorie");
    }
}
