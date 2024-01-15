using FluentValidation;

namespace Norexia.Core.Application.Quotations.Commands.CreateQuotation;

public class CreateQuoationCommandValidator : AbstractValidator<CreateQuotationCommand>
{
    public CreateQuoationCommandValidator()
    {
        RuleFor(t => t.Reference)
       .NotNull()
       .NotEmpty();

        RuleFor(t => t.QuotationDate)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.ValidityDuretion)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.Responsable)
        .NotNull()
        .NotEmpty();

        RuleFor(t => t.Version)
            .NotNull()
            .NotEmpty();

            

        RuleFor(t => t.QuotationLines)
            .NotNull()
            .Must(l => l!.Count > 0)
            .WithMessage("The number of lines is insufficient")
            .ForEach(l =>
            {
                l.Must(l => l.Id != Guid.Empty).WithMessage("The line id is required");
            });

        
    }
}


