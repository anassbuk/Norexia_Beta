using FluentValidation;

namespace Norexia.Core.Application.Deliveries.Commands.UpdateDelivery;
public class UpdateDeliveryCommandValidator : AbstractValidator<UpdateDeliveryCommand>
{
    public UpdateDeliveryCommandValidator()
    {
        RuleFor(t => t.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.DelivererId)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.Reference)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.EntryDate)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.DeliveryLines)
            .NotNull()
            .Must(l => l!.Count > 0)
            .WithMessage("The number of lines is insufficient");
    }
}
