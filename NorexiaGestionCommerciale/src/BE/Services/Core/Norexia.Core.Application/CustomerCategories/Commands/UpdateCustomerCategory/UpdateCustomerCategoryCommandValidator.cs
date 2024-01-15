using FluentValidation;

namespace Norexia.Core.Application.CustomerCategories.Commands.UpdateCustomerCategory;
public class UpdateCustomerCategoryCommandValidator : AbstractValidator<UpdateCustomerCategoryCommand>
{
    public UpdateCustomerCategoryCommandValidator()
    {
        RuleFor(t => t.Name)
            .MaximumLength(100)
                .NotEmpty();
    }
}
