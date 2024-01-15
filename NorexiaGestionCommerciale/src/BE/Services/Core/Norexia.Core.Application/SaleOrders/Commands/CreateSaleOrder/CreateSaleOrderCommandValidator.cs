using FluentValidation;

using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.SaleOrders.Commands.CreateSaleOrder;
public class CreateSaleOrderCommandValidator : AbstractValidator<CreateSaleOrderCommand>
{
    public CreateSaleOrderCommandValidator()
    {
        //RuleFor(t => t.CustomerId)
        //    .NotNull()
        //    .NotEmpty();

        RuleFor(t => t.Reference)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.OperationType)
            .NotNull();

        RuleFor(t => t.Execution)
            .NotNull();

        RuleFor(t => t.SaleOrderOrigin)
            .NotNull();

        When(t => t.SaleOrderOrigin == SaleOrderOrigin.Quotation, () =>
        {
            RuleFor(t => t.QuotationId)
                .NotEmpty();
        });

        RuleFor(t => t.OrderDate)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.SaleOrderLines)
            .NotNull()
            .Must(l => l!.Count > 0)
            .WithMessage("The number of lines is insufficient");
    }
}
