using FluentValidation;

namespace Norexia.Core.Application.CustomerCategories.Commands.CreateCustomerCategory;
public class CreateCustomerCategoryCommandValidator : AbstractValidator<CreateCustomerCategoryCommand>
{
    public CreateCustomerCategoryCommandValidator()
    {
        RuleFor(t => t.Name)
            .MaximumLength(128)
            .NotEmpty();
    }
}
