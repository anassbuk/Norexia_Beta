using FluentValidation;

using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.SaleOrders.Commands.UpdateSaleOrder;
public class UpdateSaleOrderCommandValidator : AbstractValidator<UpdateSaleOrderCommand>
{
    public UpdateSaleOrderCommandValidator()
    {
        RuleFor(t => t.OperationType)
            .NotNull();

        RuleFor(t => t.Execution)
            .NotNull();

        RuleFor(t => t.Reference)
            .NotNull()
            .NotEmpty();

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
