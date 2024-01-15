using FluentValidation;

namespace Norexia.Core.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {

        RuleFor(t => t.ShortDesignation)
            .MaximumLength(100)
            .NotEmpty();

        RuleFor(t => t.LongDesignation)
            .MaximumLength(1000);


        When(t => t.StorageSupplyInfo != null,
            () =>
            RuleFor
            (c => c.StorageSupplyInfo!.Quantity)
            .GreaterThanOrEqualTo(0));
    }
}
