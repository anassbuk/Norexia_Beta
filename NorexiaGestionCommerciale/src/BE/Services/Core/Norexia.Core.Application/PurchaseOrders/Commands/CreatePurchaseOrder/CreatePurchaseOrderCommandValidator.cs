using FluentValidation;

namespace Norexia.Core.Application.PurchaseOrders.Commands.CreatePurchaseOrder;
public class CreatePurchaseOrderCommandValidator : AbstractValidator<CreatePurchaseOrderCommand>
{
    public CreatePurchaseOrderCommandValidator()
    {

        RuleFor(t => t.ProviderId)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.PurchaseOrderLines)
            .NotNull()
            .Must(l => l!.Count > 0)
            .WithMessage("The number of lines is insufficient");
    }
}