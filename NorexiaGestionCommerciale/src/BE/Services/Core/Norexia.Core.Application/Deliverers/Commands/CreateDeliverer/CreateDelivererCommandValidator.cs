using FluentValidation;

namespace Norexia.Core.Application.Deliverers.Commands.CreateDeliverer;
public class CreateDelivererCommandValidator : AbstractValidator<CreateDelivererCommand>
{
    public CreateDelivererCommandValidator()
    {

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
