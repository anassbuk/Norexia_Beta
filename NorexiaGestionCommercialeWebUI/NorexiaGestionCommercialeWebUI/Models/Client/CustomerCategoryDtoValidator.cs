using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Client
{
    public class CustomerCategoryDtoValidator : AbstractValidator<CustomerCategoryDto>
    {
        public CustomerCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .NotEmpty()
                .WithName("Catégorie");
        }
    }
}
