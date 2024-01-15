using FluentValidation;

namespace Norexia.Core.Application.Quotations.Commands.UpdateQuotation;

internal class UpdateQuotationCommandValidator : AbstractValidator<UpdateQuotationCommand>
{
    public UpdateQuotationCommandValidator()
    {
        RuleFor(t => t.Reference)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.QuotationDate)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.QuotationLines)
            .NotNull()
            .Must(l => l!.Count > 0)
            .WithMessage("The number of lines is insufficient");
    }
}

