using FluentValidation;

namespace Norexia.Core.Application.Deliverers.Commands.UpdateDeliverer;
public class UpdateDelivererCommandValidator : AbstractValidator<UpdateDelivererCommand>
{
    public UpdateDelivererCommandValidator()
    {
        RuleFor(t => t.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.Reference)
            .NotEmpty();

        RuleFor(t => t.Type)
            .NotNull();

        RuleFor(t => t.LastName)
           .MaximumLength(100)
           .NotEmpty();

        RuleFor(t => t.FirstName)
           .MaximumLength(100)
           .NotEmpty();

        RuleFor(t => t.Tel)
          .MaximumLength(20)
            .NotEmpty();
    }
}
