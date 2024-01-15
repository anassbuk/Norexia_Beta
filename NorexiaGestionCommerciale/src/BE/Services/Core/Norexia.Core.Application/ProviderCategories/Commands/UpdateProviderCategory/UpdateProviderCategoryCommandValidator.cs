using FluentValidation;

namespace Norexia.Core.Application.ProviderCategories.Commands.UpdateProviderCategory;
public class UpdateProviderCategoryCommandValidator : AbstractValidator<UpdateProviderCategoryCommand>
{
    public UpdateProviderCategoryCommandValidator()
    {
        RuleFor(t => t.Name)
            .MaximumLength(100)
                .NotEmpty();
    }
}
