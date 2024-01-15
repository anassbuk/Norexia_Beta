using FluentValidation;

namespace Norexia.Core.Application.ProviderCategories.Commands.CreateProviderCategory;
public class CreateProviderCategoryCommandValidator : AbstractValidator<CreateProviderCategoryCommand>
{
    public CreateProviderCategoryCommandValidator()
    {
        RuleFor(t => t.Name)
            .MaximumLength(128)
                .NotEmpty();
    }
}
