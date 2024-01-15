using FluentValidation;

namespace Norexia.Core.Application.Invoices.Commands.CreateInvoice;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(t => t.Reference)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.EntryDate)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.InvoiceLines)
            .NotNull()
            .Must(l => l!.Count > 0)
            .WithMessage("The number of lines is insufficient");
    }
}
