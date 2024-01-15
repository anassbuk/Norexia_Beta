using FluentValidation;

namespace Norexia.Core.Application.Products.Commands.UpdateProduct;
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(t => t.ShortDesignation)
            .MaximumLength(100)
            .NotEmpty();

        RuleFor(t => t.LongDesignation)
            .MaximumLength(1000);

        RuleFor(t => t.Description)
            .MaximumLength(2500);

        RuleFor(t => t.StorageSupplyInfo!.Quantity)
            .GreaterThan(0);
    }
}
